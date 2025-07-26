using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interface;
using Domain.Entities;
using Application.Common.DTO;
using Application.Common.DTO.Key;
using Domain.Enum;
using Application.Common.DTO.KeyUsageDto;
using StackExchange.Redis;

namespace Domain.Repository;

public class KeyRepo : IKeyRepo
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IConnectionMultiplexer _redis;
    private const string UsageScoresKey = "key_usage_scores";


    public KeyRepo(IDbContextFactory<ApplicationDbContext> contextFactory, IConnectionMultiplexer redis)
    {
        _contextFactory = contextFactory;
        _redis = redis;
    }

    public async Task<int> CreateKey(Key key, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await _context.Key.AddAsync(key, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> UpdateKey(Key key, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        _context.Key.Update(key);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Key?> GetKeyById(Guid id, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.Key
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PaginationListDTO<List<KeyDto>>> GetListKeys(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Key.AsQueryable();

        var itemsToSkip = (pageNumber - 1) * pageSize;

        // Count total
        var totalCount = await query.CountAsync();

        List<KeyDto> items = await query
        .Include(x => x.Status)
        .OrderBy(x => x.CreatedAt)
        .Skip(itemsToSkip)
        .Take(pageSize)
        .Select(key => new KeyDto
        {
            Id = key.Id,
            KeyName = key.KeyName,
            KeyValue = key.KeyValue,
            Quota = key.Quota,
            Status = key.Status.Name,
            RevokedAt = key.RevokedAt,
            CreatedAt = key.CreatedAt,
        })
        .ToListAsync();

        PaginationListDTO<List<KeyDto>> list = new PaginationListDTO<List<KeyDto>>
        {
            totalCount = totalCount,
            pageNumber = pageNumber,
            pageSize = pageSize,
            listData = items
        };

        return list;
    }

    public async Task<bool> IsNameExist(string name, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.Key
            .AnyAsync(x => x.KeyName.ToLower() == name.ToLower());
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<KeysPerUserDto>> GetKeysPerUser(CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var keysPerUser = await _context.User
        .Where(user => user.Keys.Any())
        .Select(user => new KeysPerUserDto
        {
            username = user.Username,
            totalKeysPerUser = user.Keys.Count()
        })
        .OrderByDescending(dto => dto.totalKeysPerUser)
        .ToListAsync(cancellationToken);

        return keysPerUser;
    }

    public async Task<int> GetTotalActive(CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.Key
            .CountAsync(key => key.StatusId == (int)KeyStatusEnum.Active, cancellationToken);
    }

    public async Task<int> GetTotalKeys(CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.Key.CountAsync();
    }

    public async Task<int> GetTotalRevoked(CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.Key
            .CountAsync(key => key.StatusId == (int)KeyStatusEnum.Revoked, cancellationToken);
    }

    public async Task<List<KeyIdleValueDto>> GetIdleKeys(CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        IDatabase db = _redis.GetDatabase();

        var allKeyIds = await context.Key
        .Select(key => key.Id)
        .ToListAsync(cancellationToken);

        var usedRedisKeys = await db.SortedSetRangeByRankAsync(UsageScoresKey, 0, -1);
        var usedKeyIds = usedRedisKeys.Select(k => Guid.Parse(k.ToString())).ToHashSet();

        var idleKeyIds = allKeyIds.Where(id => !usedKeyIds.Contains(id)).ToList();

        if (!idleKeyIds.Any())
        {
            return new List<KeyIdleValueDto>();
        }

        return await context.Key
            .Where(k => idleKeyIds.Contains(k.Id))
            .Select(k => new KeyIdleValueDto
            {
                keyName = k.KeyName,
                keyValue = k.KeyValue,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<KeyValueDto>> GetTopUsedKeys(int sizeOfList, CancellationToken cancellationToken)
    {
        IDatabase db = _redis.GetDatabase();

        var topKeys = await db.SortedSetRangeByRankWithScoresAsync(UsageScoresKey, 0, sizeOfList - 1, Order.Descending);

        if (topKeys == null || !topKeys.Any())
        {
            return new List<KeyValueDto>();
        }

        var usageScores = topKeys.ToDictionary(
            entry => Guid.Parse(entry.Element),
            entry => (long)entry.Score
        );

        var keyIds = usageScores.Keys.ToList();

        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await _context.Key
        .Where(key => keyIds.Contains(key.Id))
        .Select(key => new KeyValueDto
        {
            keyName = key.KeyName,
            keyValue = key.KeyValue,
            usage = usageScores[key.Id]
        })
        .ToListAsync(cancellationToken);
    }

    public async Task<Key?> GetKeyByKeyValue(string key)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Key
            .Where(x => x.KeyValue == key)
            .FirstOrDefaultAsync();
    }
}
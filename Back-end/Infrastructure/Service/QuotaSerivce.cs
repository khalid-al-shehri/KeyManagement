using System;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Memory;
using Application.Common.Interface;
using Domain.Enum;
using Domain.DTO.Key;

namespace Application.Common.Service;

public class QuotaService : IQuotaService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IKeyRepo _keyRepo;
    private readonly IMemoryCache _memoryCache;
    private readonly ILoggedUser _loggedUser;

    public QuotaService(IConnectionMultiplexer redis, IKeyRepo keyRepo, IMemoryCache memoryCache, ILoggedUser loggedUser)
    {
        _redis = redis;
        _keyRepo = keyRepo;
        _memoryCache = memoryCache;
        _loggedUser = loggedUser;
    }

    public async Task<ResultUseKey> ValidateAndIncrementUsageAsync(string keyValue)
    {

        const string usageScoresKey = "key_usage_scores";

        var key = await _memoryCache.GetOrCreateAsync(
            $"key:{keyValue}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _keyRepo.GetKeyByKeyValue(keyValue);
            }
        );


        if (key == null)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Key not exist"
            };
        }

        if (key.CreatedBy != _loggedUser.Id)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Not authorized to use this key"
            };
        }

        if (key.StatusId != (int)KeyStatusEnum.Active)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Key has been revoked!"
            };
        }

        IDatabase db = _redis.GetDatabase();

        ITransaction transaction = db.CreateTransaction();
                
        double? redisResult = await db.SortedSetScoreAsync(usageScoresKey, key.Id.ToString());
        long currentUsage = redisResult.HasValue ? (long)redisResult : 0;

        if (currentUsage >= key.Quota)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = $"Key extend allowed quota ({key.Quota} times)"
            };
        }

        transaction.SortedSetIncrementAsync(usageScoresKey, key.Id.ToString(), 1);

        bool wasSuccessful = await transaction.ExecuteAsync();

        if (!wasSuccessful)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = $"Unknown error!"
            };
        }

        return new ResultUseKey
        {
            Succeeded = true,
        };
    }
    public async Task<ResultUseKey> IsKeyValid(string keyValue)
    {
        const string usageScoresKey = "key_usage_scores";

        var key = await _memoryCache.GetOrCreateAsync(
            $"key:{keyValue}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _keyRepo.GetKeyByKeyValue(keyValue);
            });


        if (key == null)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Key not exist"
            };
        }

        if (key.CreatedBy != _loggedUser.Id)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Not authorized to check this key"
            };
        }

        if (key.StatusId != (int)KeyStatusEnum.Active)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = "Key has been revoked!"
            };
        }

        IDatabase db = _redis.GetDatabase();
        double? redisResult = await db.SortedSetScoreAsync(usageScoresKey, key.Id.ToString());
        long currentUsage = redisResult.HasValue ? (long)redisResult : 0;

        if (currentUsage >= key.Quota)
        {
            return new ResultUseKey
            {
                Succeeded = false,
                errorMessage = $"Key extend allowed quota ({key.Quota} times)"
            };
        }

        return new ResultUseKey
        {
            Succeeded = true,
        };
    }

    public void RemoveKeyQuota(string keyValue)
    {
        string cacheKey = $"key:{keyValue}";
        _memoryCache.Remove(cacheKey);
    }
}
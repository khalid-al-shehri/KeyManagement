using System;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interface;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Domain.Repository;

public class UserRepo : IUserRepo
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public UserRepo(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<User?> GetUserByUsername(string Username, CancellationToken cancellationToken)
    {
        await using var _context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await _context.User
            .Where(x => x.Username.ToLower() == Username.ToLower())
            .FirstOrDefaultAsync(cancellationToken);
    }
}
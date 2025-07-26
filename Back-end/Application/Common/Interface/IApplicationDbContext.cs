using System;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Common.Interface;

public interface IApplicationDbContext
{
    DbSet<User> User { get; }
    DbSet<Key> Key { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
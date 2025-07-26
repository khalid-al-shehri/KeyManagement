using System;
using Domain.Entities;

namespace Application.Common.Interface;

public interface IUserRepo
{
    Task<User?> GetUserByUsername(string Username, CancellationToken cancellationToken);
}

using System;
using Domain.DTO.Key;

namespace Application.Common.Interface;

public interface IQuotaService
{
    Task<ResultUseKey> ValidateAndIncrementUsageAsync(string keyValue);
    Task<ResultUseKey> IsKeyValid(string keyValue);
    void RemoveKeyQuota(string keyValue);
    
}
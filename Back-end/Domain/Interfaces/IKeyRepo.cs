using System;
using Application.Common.DTO;
using Application.Common.DTO.Key;
using Application.Common.DTO.KeyUsageDto;
using Domain.Entities;

namespace Application.Common.Interface;

public interface IKeyRepo
{
    Task<PaginationListDTO<List<KeyDto>>> GetListKeys(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Key?> GetKeyById(Guid id, CancellationToken cancellationToken);
    Task<Key?> GetKeyByKeyValue(string key);
    Task<bool> IsNameExist(string name, CancellationToken cancellationToken);
    Task<int> UpdateKey(Key key, CancellationToken cancellationToken);
    Task<int> CreateKey(Key key, CancellationToken cancellationToken);
    Task<int> GetTotalKeys(CancellationToken cancellationToken);
    Task<List<KeysPerUserDto>> GetKeysPerUser(CancellationToken cancellationToken);
    Task<int> GetTotalActive(CancellationToken cancellationToken);
    Task<int> GetTotalRevoked(CancellationToken cancellationToken);
    Task<List<KeyValueDto>> GetTopUsedKeys(int sizeOfList, CancellationToken cancellationToken);
    Task<List<KeyIdleValueDto>> GetIdleKeys(CancellationToken cancellationToken);



}

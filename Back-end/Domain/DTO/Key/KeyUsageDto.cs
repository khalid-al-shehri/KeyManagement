
namespace Application.Common.DTO.KeyUsageDto;

public record class KeyUsageDto
{
    public required int totalKeys { get; set; }
    public required List<KeysPerUserDto> keysPerUser { get; set; }
    public required KeysStatusDto keysStatus { get; set; }
    public required List<KeyValueDto> topUsedKeys { get; set; }
    public required List<KeyIdleValueDto> idleKeys { get; set; }
}

public record class KeysPerUserDto
{
    public required string username { get; set; }
    public required int totalKeysPerUser { get; set; }
}

public record class KeysStatusDto
{
    public required int totalRevokedKeys { get; set; }
    public required int totalActiveKeys { get; set; }
}

public record class KeyValueDto
{
    public required string keyName { get; set; }
    public required string keyValue { get; set; }
    public required long usage { get; set; }
}

public record class KeyIdleValueDto
{
    public required string keyName { get; set; }
    public required string keyValue { get; set; }
}


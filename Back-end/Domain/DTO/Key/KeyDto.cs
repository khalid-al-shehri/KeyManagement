namespace Application.Common.DTO.Key;

public record class KeyDto
{
    public required Guid Id { get; set; }
    public required string KeyName { get; set; }
    public required string KeyValue { get; set; }
    public required int Quota { get; set; }
    public required string Status { get; set; }
    public DateOnly? RevokedAt { get; set; }
    public required DateTime CreatedAt { get; set; }
}

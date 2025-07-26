using Domain.Common;

namespace Domain.Entities;

public class Key : BaseAuditableEntity<Guid, Key>
{
    public required string KeyName { get; set; }
    public required string KeyValue { get; set; }
    public required int Quota { get; set; }
    public required int StatusId { get; set; }
    public DateOnly? RevokedAt { get; set; }
    public Status Status { get; set; }
}

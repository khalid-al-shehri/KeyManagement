using Domain.Common;

namespace Domain.Entities;

public class User : BaseAuditableEntity<Guid, User>
{
    public required string Username { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<Key> Keys { get; set; }
}

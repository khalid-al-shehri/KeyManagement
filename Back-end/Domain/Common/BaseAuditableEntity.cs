using System;

namespace Domain.Common;

public abstract class BaseAuditableEntity<T> : BaseEntity
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}


public abstract class BaseAuditableEntity<TId, TSource> : BaseEntity
{
    public TId Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid TrackUUId { get; set; } = Guid.NewGuid();


    public TSource ShallowCopy()
    {
        return (TSource)this.MemberwiseClone();
    }

}
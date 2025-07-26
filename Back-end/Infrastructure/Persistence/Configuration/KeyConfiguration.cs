using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configuration;

public class KeyConfiguration : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder)
    {
        // BaseAuditableEntity
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedNever();

        // Unique and Index
        builder.HasIndex(x => x.KeyName)
        .IsUnique();
        
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.CreatedAt)
        .IsRequired(true)
        .HasColumnType("datetime");

        builder.Property(x => x.CreatedBy)
        .IsRequired(false);

        builder.Property(x => x.LastModified)
        .IsRequired(false)
        .HasColumnType("datetime");

        builder.Property(x => x.LastModifiedBy)
        .IsRequired(false);

        builder.Property(x => x.TrackUUId)
        .IsRequired(true);

        // Entity Schema
        builder.Property(x => x.KeyName)
        .IsRequired(true);

        builder.Property(x => x.KeyValue)
        .IsRequired(true);

        builder.Property(x => x.Quota)
        .IsRequired(true);
        
        builder.Property(x => x.RevokedAt)
        .IsRequired(false);
        
        builder.HasOne(x => x.Status)
        .WithMany()
        .HasForeignKey(x => x.StatusId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}

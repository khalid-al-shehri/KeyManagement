using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // BaseAuditableEntity
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedNever();

        // Unique and Index
        builder.HasIndex(x => x.Username)
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

        // Entity Schema
        builder.Property(x => x.Username)
        .IsRequired(true);

        builder.Property(x => x.FullName)
        .IsRequired(true);

        builder.Property(x => x.IsActive)
        .IsRequired(true);
        
        builder.HasMany(x => x.Keys)
        .WithOne()
        .HasForeignKey(x => x.CreatedBy)
        .OnDelete(DeleteBehavior.Restrict);
    }
}

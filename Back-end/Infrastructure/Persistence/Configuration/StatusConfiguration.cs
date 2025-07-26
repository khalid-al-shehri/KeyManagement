using System;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedNever();
        
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.Name)
        .IsRequired(true);

        builder.Property(x => x.IsDeleted)
        .IsRequired(true);
    
        builder.HasData(GetSeededData());
    }

    private List<Status> GetSeededData()
    {
        return new List<Status>
        {
            new Status { Id = (int)KeyStatusEnum.Active, Name = nameof(KeyStatusEnum.Active)},
            new Status { Id = (int)KeyStatusEnum.Revoked, Name = nameof(KeyStatusEnum.Revoked)},
        };
    }
}

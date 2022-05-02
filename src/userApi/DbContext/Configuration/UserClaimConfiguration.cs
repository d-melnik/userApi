using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using userApi.Entities;

namespace userApi.DbContext.Configuration;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaimEntity>
{
    public void Configure(EntityTypeBuilder<UserClaimEntity> builder)
    {
        builder.HasData(CreateDefaultUserClaims());
    }

    private IList<UserClaimEntity> CreateDefaultUserClaims()
    {
        return new List<UserClaimEntity>()
        {
            new()
            {
                Id = 1,
                ClaimId = ClaimConfiguration.AdminClaimId,
                UserId = ClaimConfiguration.AdminClaimId
            },
            new()
            {
                Id = 2,
                ClaimId = ClaimConfiguration.EditorClaimId,
                UserId = ClaimConfiguration.EditorClaimId
            },
            new()
            {
                Id = 3,
                ClaimId = ClaimConfiguration.UserClaimId,
                UserId = ClaimConfiguration.UserClaimId
            }
        };
    }
}
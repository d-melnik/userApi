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

    public static IList<UserClaimEntity> CreateDefaultUserClaims()
    {
        return new List<UserClaimEntity>()
        {
            new()
            {
                Id = 1,
                ClaimId = ClaimConfiguration.AdminClaimId,
                UserId = UserConfiguration.AdminUserId
            },
            new()
            {
                Id = 2,
                ClaimId = ClaimConfiguration.EditorClaimId,
                UserId = UserConfiguration.EditorUserId
            },
            new()
            {
                Id = 3,
                ClaimId = ClaimConfiguration.UserClaimId,
                UserId = UserConfiguration.UserUserId
            }
        };
    }
}
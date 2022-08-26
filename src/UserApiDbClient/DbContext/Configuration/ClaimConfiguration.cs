using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserApiDbClient.Entities;

namespace UserApiDbClient.DbContext.Configuration;

public class ClaimConfiguration: IEntityTypeConfiguration<ClaimEntity>
{
    public const string AdminRoleName = "Admin Role";
    public const string EditorRoleName = "Editor Role";
    
    public const int AdminClaimId = 1;
    public const int EditorClaimId = 2;
    public const int UserClaimId = 3;
    public void Configure(EntityTypeBuilder<ClaimEntity> builder)
    {
        builder.HasData(CreateDefaultClaims());
    }

    public static IList<ClaimEntity> CreateDefaultClaims()
    {
        return new List<ClaimEntity>()
        {
            new()
            {
                Id = AdminClaimId,
                Name = AdminRoleName,
                Description = "For Admin"
            },
            new()
            {
                Id = EditorClaimId,
                Name = EditorRoleName,
                Description = "For Editor"
            },
            new()
            {
                Id = UserClaimId,
                Name = "User Role",
                Description = "For User"
            }
        };
    }
}
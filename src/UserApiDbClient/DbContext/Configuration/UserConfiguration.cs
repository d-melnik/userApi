using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserApiDbClient.Entities;

namespace UserApiDbClient.DbContext.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public const int AdminUserId = 1;
    public const int EditorUserId = 2;
    public const int UserUserId = 3;

    public const string defaultPwd = "Pass!234";
    
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasData(CreateDefaultUsers());
    }

    public static IList<UserEntity> CreateDefaultUsers()
    {
        return new List<UserEntity>()
        {
            new()
            {
                Id = AdminUserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "admin@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(defaultPwd)
            },
            new()
            {
                Id = EditorUserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "editor@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(defaultPwd)
            },
            new()
            {
                Id = UserUserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "user@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(defaultPwd)
            }
        };
    }
}
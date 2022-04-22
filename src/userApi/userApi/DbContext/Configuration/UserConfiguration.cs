using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using userApi.Entities;

namespace userApi.DbContext.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasData(CreateDefaultUser());
    }

    private IList<UserEntity> CreateDefaultUser()
    {
        return new List<UserEntity>()
        {
            new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "test@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                Password = "Pass!234"
            },
            new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "editor@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                Password = "Pass!234"
            },
            new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Email = "admin@test.com",
                FirstName = "testFirst",
                LastName = "testLast",
                Password = "Pass!234"
            }
        };
    }
}
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using userApi.DbContext.Configuration;
using userApi.Entities;
using userApi.Helpers;

namespace userApi.DbContext
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration configuration;

        public DataContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            Version mySqlVersion = GetMySqlVersionFromConfigHelper.GetVersion(configuration.GetConnectionString("Version"));
            // connect to sql server database
            options.UseMySql(configuration.GetConnectionString("WebApiDatabase"), 
                new MySqlServerVersion(mySqlVersion));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaimEntity>()
                .HasKey(uc => new { uc.UserId, uc.ClaimId });

            modelBuilder.Entity<UserClaimEntity>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserClaim)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserClaimEntity>()
                .HasOne(uc => uc.Claim)
                .WithMany(c => c.UserClaim)
                .HasForeignKey(uc => uc.ClaimId);
            
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
    }
}
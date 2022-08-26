using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;

namespace UserApiDbClient.DbContext
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration configuration;
        
        public DataContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            this.configuration = configuration;
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

        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<ClaimEntity>? Claims { get; set; }
    }
}
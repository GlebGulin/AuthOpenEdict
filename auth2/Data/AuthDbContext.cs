using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auth2.Data
{
    public class AuthDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUserSetting> ApplicationUserSetting => Set<ApplicationUserSetting>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserSetting>(entity =>
            {
                entity.ToTable("ApplicationUserSetting");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(x => x.Value)
                      .HasMaxLength(512);

                entity.Property(x => x.Type)
                      .HasMaxLength(50);

                entity.HasOne(x => x.User)
                      .WithMany(u => u.Settings)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

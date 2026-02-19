using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("ProjectIdSequence")
                .IncrementsBy(100);
            
            builder.Entity<Project>()
                .Property(p => p.Id)
                .UseHiLo("ProjectIdSequence");

            //builder.Entity<Project>().HasQueryFilter(p => p.DeletedAt.HasValue != false);

            builder.Entity<Project>()
                .HasIndex(p => p.DeletedAt)
                .HasFilter("\"DeletedAt\" IS NULL");

            builder.Entity<ProjectComment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectReaction>()
                .Property(r => r.Type)
                .HasConversion<string>();

            builder.Entity<Notification>()
                .Property(r => r.Type)
                .HasConversion<string>();

            builder.Entity<UserDailyUploadStats>()
                .HasKey(u => new { u.UserId, u.Date });
        }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTimestamps()
        {
            var entries = ChangeTracker.Entries<ITrackable>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectReaction> ProjectLikes { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
        public DbSet<UserBan> UserBans { get; set; }
        public DbSet<ProjectReport> ProjectReports { get; set; }
        public DbSet<ProjectBan> ProjectBans { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<UserDailyUploadStats> UserDailyUploadStats { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}

using Scratch.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Scratch.Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
            var entries = this.ChangeTracker.Entries<ITrackable>();

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
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectLike> ProjectLikes { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data
{
    public class TennisAcademyDbContext : IdentityDbContext<ApplicationUser>
    {
        // Премахваме FacultyNumber от името на схемата, тъй като в Postgres това 
        // изисква специфични права и структура, които често водят до грешки.

        public TennisAcademyDbContext(DbContextOptions<TennisAcademyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coach> Coaches { get; set; } = null!;
        public virtual DbSet<Surface> Surfaces { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<TrainingType> Trainings { get; set; } = null!;
        public virtual DbSet<UserFavourite> UserFavourites { get; set; } = null!;
        public virtual DbSet<Racket> Rackets { get; set; } = null!;
        public virtual DbSet<RacketCart> RacketCart { get; set; } = null!;
        public virtual DbSet<Ball> Balls { get; set; } = null!;
        public virtual DbSet<BallCart> BallCart { get; set; } = null!;
        public virtual DbSet<Bag> Bags { get; set; } = null!;
        public virtual DbSet<BagCart> BagCart { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<TournamentCategory> TournamentCategories { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<TournamentUser> TournamentsUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder config)
        {
            base.OnModelCreating(config);

            string auditColumnName = "LastModified";

            foreach (var entityType in config.Model.GetEntityTypes())
            {
                if (entityType.ClrType.Namespace != null &&
                    entityType.ClrType.Namespace.StartsWith("Microsoft.AspNetCore.Identity"))
                {
                    continue;
                }

                if (entityType.ClrType == typeof(ApplicationUser) || entityType.ClrType == typeof(AuditLog))
                {
                    continue;
                }

                config.Entity(entityType.ClrType)
                    .Property<DateTime>(auditColumnName)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            }

            config.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            string auditColumnName = "LastModified";

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Metadata.ClrType.Namespace != null &&
                    entry.Metadata.ClrType.Namespace.StartsWith("Microsoft.AspNetCore.Identity"))
                {
                    continue;
                }

                if (entry.Entity is AuditLog || entry.Entity is ApplicationUser)
                {
                    continue;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property(auditColumnName).CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
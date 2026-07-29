using System;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;
        public DbSet<CandidatePii> CandidatePiis { get; set; } = null!;
        public DbSet<CandidateExperience> CandidateExperiences { get; set; } = null!;
        public DbSet<CandidateQualification> CandidateQualifications { get; set; } = null!;
        public DbSet<CandidateDocument> CandidateDocuments { get; set; } = null!;
        public DbSet<ProfileVersion> ProfileVersions { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Batch> Batches { get; set; } = null!;
        public DbSet<Grant> Grants { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<AccessEvent> AccessEvents { get; set; } = null!;
        public DbSet<Consent> Consents { get; set; } = null!;
        public DbSet<Outcome> Outcomes { get; set; } = null!;
        public DbSet<FollowUpTask> FollowUpTasks { get; set; } = null!;
        public DbSet<MaskingRule> MaskingRules { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<JobCategory> JobCategories { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
        public DbSet<CandidateCategory> CandidateCategories { get; set; } = null!;
        public DbSet<CompanyNote> CompanyNotes { get; set; } = null!;
        public DbSet<AccessRequest> AccessRequests { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // String enum conversions
            modelBuilder.Entity<User>().Property(e => e.Role).HasConversion<string>();
            modelBuilder.Entity<User>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<CandidateProfile>().Property(e => e.ExperienceBand).HasConversion<string>();
            modelBuilder.Entity<CandidateProfile>().Property(e => e.Availability).HasConversion<string>();
            modelBuilder.Entity<CandidateProfile>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<CandidateQualification>().Property(e => e.Level).HasConversion<string>();
            modelBuilder.Entity<CandidateDocument>().Property(e => e.DocumentType).HasConversion<string>();
            modelBuilder.Entity<CandidateDocument>().Property(e => e.ScanStatus).HasConversion<string>();
            modelBuilder.Entity<Company>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Batch>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Grant>().Property(e => e.Scope).HasConversion<string>();
            modelBuilder.Entity<Grant>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(e => e.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<AccessEvent>().Property(e => e.EventType).HasConversion<string>();
            modelBuilder.Entity<Outcome>().Property(e => e.OutcomeValue).HasConversion<string>();
            modelBuilder.Entity<Outcome>().Property(e => e.ReportedVia).HasConversion<string>();
            modelBuilder.Entity<FollowUpTask>().Property(e => e.TaskType).HasConversion<string>();
            modelBuilder.Entity<FollowUpTask>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<MaskingRule>().Property(e => e.RuleType).HasConversion<string>();
            modelBuilder.Entity<MaskingRule>().Property(e => e.ReplacementStrategy).HasConversion<string>();
            modelBuilder.Entity<AccessRequest>().Property(e => e.Status).HasConversion<string>();

            // Unique Indexes
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<CandidateProfile>().HasIndex(c => c.ReferenceCode).IsUnique();
            modelBuilder.Entity<CandidatePii>().HasIndex(c => c.NicNumber).IsUnique();
            modelBuilder.Entity<Company>().HasIndex(c => c.BusinessRegNo).IsUnique();
            modelBuilder.Entity<Batch>().HasIndex(b => b.BatchCode).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(o => o.OrderCode).IsUnique();
            modelBuilder.Entity<Setting>().HasIndex(s => s.Key).IsUnique();

            // One-to-one
            modelBuilder.Entity<CandidatePii>()
                .HasOne(p => p.CandidateProfile)
                .WithOne()
                .HasForeignKey<CandidatePii>(p => p.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Outcome>()
                .HasOne(o => o.Grant)
                .WithOne(g => g.Outcome)
                .HasForeignKey<Outcome>(o => o.GrantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            modelBuilder.Entity<Grant>()
                .HasIndex(g => g.CandidateProfileId)
                .HasFilter("Status = 'Active'")
                .IsUnique();

            modelBuilder.Entity<Grant>()
                .HasIndex(g => new { g.CompanyId, g.CandidateProfileId, g.Status, g.ValidUntil });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var createdAtProp = entityEntry.Metadata.FindProperty("CreatedAt");
                var updatedAtProp = entityEntry.Metadata.FindProperty("UpdatedAt");

                if (entityEntry.State == EntityState.Added && createdAtProp != null)
                {
                    entityEntry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified && updatedAtProp != null)
                {
                    entityEntry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

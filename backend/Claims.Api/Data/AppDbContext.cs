using Claims.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<InsuranceClaim> Claims => Set<InsuranceClaim>();

    public DbSet<ClaimDocument> ClaimDocuments => Set<ClaimDocument>();

    public DbSet<ClaimAssignment> ClaimAssignments => Set<ClaimAssignment>();

    public DbSet<ClaimStatusHistory> ClaimStatusHistories
        => Set<ClaimStatusHistory>();

    public DbSet<ClaimAssessment> ClaimAssessments
        => Set<ClaimAssessment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Role)
                .HasConversion<string>();
        });

        // Claim configuration
        modelBuilder.Entity<InsuranceClaim>(entity =>
        {
            entity.HasIndex(c => c.ClaimNumber).IsUnique();

            entity.HasIndex(c => c.Status);

            entity.Property(c => c.ClaimType)
                .HasConversion<string>();

            entity.Property(c => c.Status)
                .HasConversion<string>();

            entity.HasOne(c => c.Claimant)
                .WithMany(u => u.SubmittedClaims)
                .HasForeignKey(c => c.ClaimantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.AssignedOfficer)
                .WithMany(u => u.AssignedClaims)
                .HasForeignKey(c => c.AssignedOfficerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Claim document configuration
        modelBuilder.Entity<ClaimDocument>(entity =>
        {
            entity.HasOne(d => d.InsuranceClaim)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.InsuranceClaimId);

            entity.HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Assignment configuration
        modelBuilder.Entity<ClaimAssignment>(entity =>
        {
            entity.HasOne(a => a.InsuranceClaim)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.InsuranceClaimId);

            entity.HasOne(a => a.AssignedToUser)
                .WithMany(u => u.AssignmentsReceived)
                .HasForeignKey(a => a.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.AssignedByUser)
                .WithMany(u => u.AssignmentsMade)
                .HasForeignKey(a => a.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Status history configuration
        modelBuilder.Entity<ClaimStatusHistory>(entity =>
        {
            entity.Property(h => h.PreviousStatus)
                .HasConversion<string>();

            entity.Property(h => h.NewStatus)
                .HasConversion<string>();

            entity.HasOne(h => h.InsuranceClaim)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(h => h.InsuranceClaimId);

            entity.HasOne(h => h.ChangedByUser)
                .WithMany(u => u.StatusChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Assessment configuration
        modelBuilder.Entity<ClaimAssessment>(entity =>
        {
            entity.Property(a => a.Decision)
                .HasConversion<string>();

            entity.HasOne(a => a.InsuranceClaim)
                .WithMany(c => c.Assessments)
                .HasForeignKey(a => a.InsuranceClaimId);

            entity.HasOne(a => a.AssessedByUser)
                .WithMany(u => u.Assessments)
                .HasForeignKey(a => a.AssessedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
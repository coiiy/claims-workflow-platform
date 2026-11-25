using System.ComponentModel.DataAnnotations;
using Claims.Api.Enums;

namespace Claims.Api.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(180)]
    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<InsuranceClaim> SubmittedClaims { get; set; }
        = new List<InsuranceClaim>();

    public ICollection<InsuranceClaim> AssignedClaims { get; set; }
        = new List<InsuranceClaim>();

    public ICollection<ClaimAssignment> AssignmentsMade { get; set; }
        = new List<ClaimAssignment>();

    public ICollection<ClaimAssignment> AssignmentsReceived { get; set; }
        = new List<ClaimAssignment>();

    public ICollection<ClaimStatusHistory> StatusChanges { get; set; }
        = new List<ClaimStatusHistory>();

    public ICollection<ClaimAssessment> Assessments { get; set; }
        = new List<ClaimAssessment>();
}
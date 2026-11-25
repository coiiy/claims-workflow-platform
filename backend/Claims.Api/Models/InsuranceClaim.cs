using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Claims.Api.Enums;

namespace Claims.Api.Models;

public class InsuranceClaim
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string ClaimNumber { get; set; } = string.Empty;

    public int ClaimantId { get; set; }

    public AppUser Claimant { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string PolicyNumber { get; set; } = string.Empty;

    public ClaimType ClaimType { get; set; }

    public DateTime IncidentDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string IncidentLocation { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string IncidentDescription { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedLoss { get; set; }

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = "MYR";

    public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

    public int? AssignedOfficerId { get; set; }

    public AppUser? AssignedOfficer { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ClaimDocument> Documents { get; set; }
        = new List<ClaimDocument>();

    public ICollection<ClaimAssignment> Assignments { get; set; }
        = new List<ClaimAssignment>();

    public ICollection<ClaimStatusHistory> StatusHistory { get; set; }
        = new List<ClaimStatusHistory>();

    public ICollection<ClaimAssessment> Assessments { get; set; }
        = new List<ClaimAssessment>();
}
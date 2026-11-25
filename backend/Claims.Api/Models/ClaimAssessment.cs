using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Claims.Api.Enums;

namespace Claims.Api.Models;

public class ClaimAssessment
{
    public int Id { get; set; }

    public int InsuranceClaimId { get; set; }

    public InsuranceClaim InsuranceClaim { get; set; } = null!;

    public int AssessedByUserId { get; set; }

    public AppUser AssessedByUser { get; set; } = null!;

    public AssessmentDecision Decision { get; set; }
        = AssessmentDecision.Pending;

    [Required]
    [MaxLength(2000)]
    public string Findings { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? RecommendedSettlementAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AssessedAt { get; set; }
}
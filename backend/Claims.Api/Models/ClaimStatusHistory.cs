using System.ComponentModel.DataAnnotations;
using Claims.Api.Enums;

namespace Claims.Api.Models;

public class ClaimStatusHistory
{
    public int Id { get; set; }

    public int InsuranceClaimId { get; set; }

    public InsuranceClaim InsuranceClaim { get; set; } = null!;

    public ClaimStatus? PreviousStatus { get; set; }

    public ClaimStatus NewStatus { get; set; }

    public int ChangedByUserId { get; set; }

    public AppUser ChangedByUser { get; set; } = null!;

    [MaxLength(1000)]
    public string? Note { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
using System.ComponentModel.DataAnnotations;

namespace Claims.Api.Models;

public class ClaimAssignment
{
    public int Id { get; set; }

    public int InsuranceClaimId { get; set; }

    public InsuranceClaim InsuranceClaim { get; set; } = null!;

    public int AssignedToUserId { get; set; }

    public AppUser AssignedToUser { get; set; } = null!;

    public int AssignedByUserId { get; set; }

    public AppUser AssignedByUser { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UnassignedAt { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
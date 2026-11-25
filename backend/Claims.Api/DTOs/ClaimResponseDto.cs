using Claims.Api.Enums;

namespace Claims.Api.DTOs;

public class ClaimResponseDto
{
    public int Id { get; set; }

    public string ClaimNumber { get; set; } = string.Empty;

    public string PolicyNumber { get; set; } = string.Empty;

    public ClaimType ClaimType { get; set; }

    public DateTime IncidentDate { get; set; }

    public string IncidentLocation { get; set; } = string.Empty;

    public string IncidentDescription { get; set; } = string.Empty;

    public decimal EstimatedLoss { get; set; }

    public string CurrencyCode { get; set; } = "MYR";

    public ClaimStatus Status { get; set; }

    public string ClaimantName { get; set; } = string.Empty;

    public string? AssignedOfficerName { get; set; }

    public DateTime SubmittedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
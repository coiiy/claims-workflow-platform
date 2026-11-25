using System.ComponentModel.DataAnnotations;
using Claims.Api.Enums;

namespace Claims.Api.DTOs;

public class CreateClaimRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ClaimantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PolicyNumber { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(ClaimType))]
    public ClaimType? ClaimType { get; set; }

    [Required]
    public DateTimeOffset? IncidentDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string IncidentLocation { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string IncidentDescription { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal EstimatedLoss { get; set; }

    [Required]
    [RegularExpression("^[A-Za-z]{3}$")]
    public string CurrencyCode { get; set; } = "MYR";
}
using System.ComponentModel.DataAnnotations;

namespace Claims.Api.Models;

public class ClaimDocument
{
    public int Id { get; set; }

    public int InsuranceClaimId { get; set; }

    public InsuranceClaim InsuranceClaim { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    [Required]
    [MaxLength(500)]
    public string StorageKey { get; set; } = string.Empty;

    public int UploadedById { get; set; }

    public AppUser UploadedBy { get; set; } = null!;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
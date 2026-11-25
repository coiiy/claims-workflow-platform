using Claims.Api.DTOs;
using Claims.Api.Enums;

namespace Claims.Api.Services;

public interface IClaimService
{
    Task<ClaimResponseDto?> CreateClaimAsync(
        CreateClaimRequest request,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<ClaimResponseDto>> GetClaimsAsync(
        int? claimantId,
        ClaimStatus? status,
        CancellationToken cancellationToken
    );

    Task<ClaimResponseDto?> GetClaimByIdAsync(
        int id,
        CancellationToken cancellationToken
    );
}
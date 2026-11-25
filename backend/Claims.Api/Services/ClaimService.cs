using Claims.Api.Data;
using Claims.Api.DTOs;
using Claims.Api.Enums;
using Claims.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Services;

public class ClaimService : IClaimService
{
    private readonly AppDbContext _context;

    public ClaimService(AppDbContext context)
    {
        _context = context;
    }

    // Create a new insurance claim
    public async Task<ClaimResponseDto?> CreateClaimAsync(
        CreateClaimRequest request,
        CancellationToken cancellationToken
    )
    {
        var claimant = await _context.Users
            .FirstOrDefaultAsync(
                u => u.Id == request.ClaimantId
                     && u.Role == UserRole.Claimant,
                cancellationToken
            );

        // Ensure the claimant exists
        if (claimant == null)
        {
            return null;
        }

        var currentTime = DateTime.UtcNow;

        var claim = new InsuranceClaim
        {
            ClaimNumber = GenerateClaimNumber(),

            ClaimantId = claimant.Id,

            PolicyNumber = request.PolicyNumber.Trim(),

            ClaimType = request.ClaimType!.Value,

            IncidentDate = request.IncidentDate!.Value.UtcDateTime,

            IncidentLocation = request.IncidentLocation.Trim(),

            IncidentDescription = request.IncidentDescription.Trim(),

            EstimatedLoss = request.EstimatedLoss,

            CurrencyCode = request.CurrencyCode.ToUpperInvariant(),

            Status = ClaimStatus.Submitted,

            SubmittedAt = currentTime,

            UpdatedAt = currentTime
        };

        // Record the initial claim status
        var statusHistory = new ClaimStatusHistory
        {
            InsuranceClaim = claim,

            PreviousStatus = null,

            NewStatus = ClaimStatus.Submitted,

            ChangedByUserId = claimant.Id,

            Note = "Claim submitted."
        };

        _context.Claims.Add(claim);

        _context.ClaimStatusHistories.Add(statusHistory);

        await _context.SaveChangesAsync(cancellationToken);

        return MapToResponse(claim, claimant.FullName, null);
    }

    // Retrieve claims with optional filters
    public async Task<IReadOnlyList<ClaimResponseDto>> GetClaimsAsync(
        int? claimantId,
        ClaimStatus? status,
        CancellationToken cancellationToken
    )
    {
        var query = _context.Claims
            .AsNoTracking()
            .Include(c => c.Claimant)
            .Include(c => c.AssignedOfficer)
            .AsQueryable();

        if (claimantId.HasValue)
        {
            query = query.Where(
                c => c.ClaimantId == claimantId.Value
            );
        }

        if (status.HasValue)
        {
            query = query.Where(
                c => c.Status == status.Value
            );
        }

        var claims = await query
            .OrderByDescending(c => c.SubmittedAt)
            .ToListAsync(cancellationToken);

        return claims
            .Select(c => MapToResponse(
                c,
                c.Claimant.FullName,
                c.AssignedOfficer?.FullName
            ))
            .ToList();
    }

    // Retrieve one claim by ID
    public async Task<ClaimResponseDto?> GetClaimByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        var claim = await _context.Claims
            .AsNoTracking()
            .Include(c => c.Claimant)
            .Include(c => c.AssignedOfficer)
            .FirstOrDefaultAsync(
                c => c.Id == id,
                cancellationToken
            );

        if (claim == null)
        {
            return null;
        }

        return MapToResponse(
            claim,
            claim.Claimant.FullName,
            claim.AssignedOfficer?.FullName
        );
    }

    // Generate a unique claim reference
    private static string GenerateClaimNumber()
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");

        var randomPart = Guid.NewGuid()
            .ToString("N")[..8]
            .ToUpperInvariant();

        return $"CLM-{date}-{randomPart}";
    }

    // Convert database entity into response DTO
    private static ClaimResponseDto MapToResponse(
        InsuranceClaim claim,
        string claimantName,
        string? officerName
    )
    {
        return new ClaimResponseDto
        {
            Id = claim.Id,

            ClaimNumber = claim.ClaimNumber,

            PolicyNumber = claim.PolicyNumber,

            ClaimType = claim.ClaimType,

            IncidentDate = claim.IncidentDate,

            IncidentLocation = claim.IncidentLocation,

            IncidentDescription = claim.IncidentDescription,

            EstimatedLoss = claim.EstimatedLoss,

            CurrencyCode = claim.CurrencyCode,

            Status = claim.Status,

            ClaimantName = claimantName,

            AssignedOfficerName = officerName,

            SubmittedAt = claim.SubmittedAt,

            UpdatedAt = claim.UpdatedAt
        };
    }
}
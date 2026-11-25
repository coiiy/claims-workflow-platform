using Claims.Api.DTOs;
using Claims.Api.Enums;
using Claims.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimService _claimService;

    public ClaimsController(IClaimService claimService)
    {
        _claimService = claimService;
    }

    // POST: api/claims
    [HttpPost]
    public async Task<ActionResult<ClaimResponseDto>> CreateClaim(
        [FromBody] CreateClaimRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _claimService.CreateClaimAsync(
            request,
            cancellationToken
        );

        if (result == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Claimant not found",
                Detail = "The specified claimant does not exist."
            });
        }

        return CreatedAtAction(
            nameof(GetClaimById),
            new { id = result.Id },
            result
        );
    }

    // GET: api/claims
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClaimResponseDto>>>
        GetClaims(
            [FromQuery] int? claimantId,
            [FromQuery] ClaimStatus? status,
            CancellationToken cancellationToken
        )
    {
        var claims = await _claimService.GetClaimsAsync(
            claimantId,
            status,
            cancellationToken
        );

        return Ok(claims);
    }

    // GET: api/claims/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClaimResponseDto>> GetClaimById(
        int id,
        CancellationToken cancellationToken
    )
    {
        var claim = await _claimService.GetClaimByIdAsync(
            id,
            cancellationToken
        );

        if (claim == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Claim not found",
                Detail = $"No claim was found with ID {id}."
            });
        }

        return Ok(claim);
    }
}
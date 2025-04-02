using flow_view_database.Rating;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace flow_view.Ratings;

public interface IRatingApiHandler
{
    Task<Results<Ok<List<RatingDTO>>, NotFound<string>>> GetAsync(CancellationToken cancellationToken);
    Task<Results<Ok<RatingDTO>, NotFound<string>>> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Ok<RatingDTO>> CreateAsync(CreateRatingDTO rating,  CancellationToken cancellationToken);
    Task<Ok<RatingDTO>> UpdateAsync(RatingDTO rating, CancellationToken cancellationToken);
    Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Results<Ok<RatingStatsDTO>, NotFound<string>>> GetStats(Guid contentId, ClaimsPrincipal userClaim, CancellationToken cancellationToken);
}

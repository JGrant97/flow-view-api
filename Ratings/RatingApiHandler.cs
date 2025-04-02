using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace flow_view.Ratings;

public class RatingApiHandler(IRatingRepository ratingRepo) : IRatingApiHandler
{
    public async Task<Ok<RatingDTO>> CreateAsync(CreateRatingDTO rating, CancellationToken cancellationToken) =>
         TypedResults.Ok((await ratingRepo.CreateAsync(rating.MapToDBModel(), cancellationToken)).MapToDTO());

    public async Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await ratingRepo.DeleteAsync(id, cancellationToken);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<List<RatingDTO>>, NotFound<string>>> GetAsync(CancellationToken cancellationToken)
    {
        var results = await ratingRepo.Get().ToListAsync();

        if (results is null || results.Count == 0)
            return TypedResults.NotFound("No ratings found.");

        return TypedResults.Ok(results.MapToDTO());
    }

    public async Task<Results<Ok<RatingDTO>, NotFound<string>>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return TypedResults.Ok((await ratingRepo.GetAsync(id, cancellationToken)).MapToDTO());
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<RatingStatsDTO>, NotFound<string>>> GetStats(Guid contentId, ClaimsPrincipal userClaim, CancellationToken cancellationToken)
    {
        var userId = userClaim.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var likes = await ratingRepo.GetLikes(contentId, cancellationToken);
            var dislikes = await ratingRepo.GetDislikes(contentId, cancellationToken);
            Rating? rating = null;

            if (userId is not null)
                rating = await ratingRepo.GetByContentIdAndUserIdAsync(contentId, new Guid(userId), cancellationToken);

            return TypedResults.Ok(new RatingStatsDTO(likes, dislikes, rating is not null ? rating.MapToDTO() : null));
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Ok<RatingDTO>> UpdateAsync(RatingDTO rating, CancellationToken cancellationToken) =>
         TypedResults.Ok((await ratingRepo.UpdateAsync(rating.MapToDBModel(), cancellationToken)).MapToDTO());
}

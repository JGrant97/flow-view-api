using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace flow_view.Ratings;

public class RatingHelper : IRatingHelper
{
    public async Task<Ok<RatingDTO>> CreateAsync(CreateRatingDTO rating, IRatingRepository ratingRepository) =>
         TypedResults.Ok((await ratingRepository.CreateAsync(rating.MapToDBModel())).MapToDTO());

    public async Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, IRatingRepository ratingRepository)
    {
        try
        {
            await ratingRepository.DeleteAsync(id);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<List<RatingDTO>>, NotFound<string>>> GetAsync(IRatingRepository ratingRepository)
    {
        var results = await ratingRepository.Get().ToListAsync();

        if (results is null || results.Count == 0)
            return TypedResults.NotFound("No ratings found.");

        return TypedResults.Ok(results.MapToDTO());
    }

    public async Task<Results<Ok<RatingDTO>, NotFound<string>>> GetAsync(Guid id, IRatingRepository ratingRepository)
    {
        try
        {
            return TypedResults.Ok((await ratingRepository.GetAsync(id)).MapToDTO());
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<RatingStatsDTO>, NotFound<string>>> GetStats(Guid contentId, IRatingRepository ratingRepository, ClaimsPrincipal userClaim)
    {
        var userId = userClaim.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var likes = ratingRepository.GetLikes(contentId);
            var dislikes = ratingRepository.GetDislikes(contentId);
            Rating? rating = null;

            if (userId is not null)
                rating = await ratingRepository.GetByContentIdAndUserIdAsync(contentId, new Guid(userId));

            return TypedResults.Ok(new RatingStatsDTO(likes, dislikes, rating is not null ? rating.MapToDTO() : null));
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Ok<RatingDTO>> UpdateAsync(RatingDTO rating, IRatingRepository ratingRepository) =>
         TypedResults.Ok((await ratingRepository.UpdateAsync(rating.MapToDBModel())).MapToDTO());
}

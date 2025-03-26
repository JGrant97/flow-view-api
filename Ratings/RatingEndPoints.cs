using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flow_view.Ratings;

public static class RatingEndPoints
{
    public static void MapRatingEndpoints(this WebApplication app)
    {
        app.MapGet("/rating", async (IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.GetAsync(ratingRepository)
        ).WithTags("Rating");

        app.MapGet("/rating/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.GetAsync(id, ratingRepository)
        ).WithTags("Rating");

        app.MapGet("/rating/stats/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository, ClaimsPrincipal userClaim) =>
            await ratinghelper.GetStats(id, ratingRepository, userClaim)
        ).WithTags("Rating");

        app.MapDelete("/rating/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.DeleteAsync(id, ratingRepository)
        ).WithTags("Rating")
        .RequireAuthorization();

        app.MapPost("/rating", async ([FromBody] CreateRatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.CreateAsync(rating, ratingRepository)
        ).WithTags("Rating")
        .RequireAuthorization();

        app.MapPut("/rating", async ([FromBody] RatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.UpdateAsync(rating, ratingRepository)
        ).WithTags("Rating")
        .RequireAuthorization();
    }
}

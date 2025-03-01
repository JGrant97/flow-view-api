using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;

namespace flow_view.Ratings;

public static class RatingEndPoints
{
    public static void MapRatingEndpoints(this WebApplication app)
    {
        app.MapGet("/rating", async (IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.GetAsync(ratingRepository)
        ).WithTags("rating");

        app.MapGet("/rating/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.GetAsync(id, ratingRepository)
        ).WithTags("rating");

        app.MapGet("/rating/delete/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.DeleteAsync(id, ratingRepository)
        ).WithTags("rating");

        app.MapGet("/rating/create", async ([FromForm] RatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.CreateAsync(rating, ratingRepository)
        ).WithTags("rating");

        app.MapGet("/rating/update", async ([FromForm] RatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.UpdateAsync(rating, ratingRepository)
        ).WithTags("rating");
    }
}

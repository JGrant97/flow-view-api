using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;

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

        app.MapDelete("/rating/delete/{id}", async (Guid id, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.DeleteAsync(id, ratingRepository)
        ).WithTags("Rating");

        app.MapPost("/rating/create", async ([FromForm] RatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.CreateAsync(rating, ratingRepository)
        ).WithTags("Rating");

        app.MapPut("/rating/update", async ([FromForm] RatingDTO rating, IRatingHelper ratinghelper, IRatingRepository ratingRepository) =>
            await ratinghelper.UpdateAsync(rating, ratingRepository)
        ).WithTags("Rating");
    }
}

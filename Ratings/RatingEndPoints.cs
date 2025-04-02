using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;

namespace flow_view.Ratings;

public static class RatingEndPoints
{
    public static void MapRatingEndpoints(this WebApplication builder)
    {
        var api = builder.MapGroup("/rating").WithTags("Rating");

        api.MapGet("/", async (IRatingApiHandler handler, CancellationToken cancellationToken) =>
            await handler.GetAsync(cancellationToken)
        );

        api.MapGet("/{id}", async (Guid id, IRatingApiHandler handler, CancellationToken cancellationToken) =>
            await handler.GetAsync(id, cancellationToken)
        );

        api.MapGet("/stats/{id}", async (Guid id, IRatingApiHandler handler, ClaimsPrincipal userClaim, CancellationToken cancellationToken) =>
            await handler.GetStats(id, userClaim, cancellationToken)
        );

        api.MapDelete("/{id}", async (Guid id, IRatingApiHandler handler, CancellationToken cancellationToken) =>
            await handler.DeleteAsync(id, cancellationToken)
        ).RequireAuthorization();

        api.MapPost("/", async ([FromBody] CreateRatingDTO rating, IRatingApiHandler handler, CancellationToken cancellationToken) =>
            await handler.CreateAsync(rating, cancellationToken)
        ).RequireAuthorization();

        api.MapPut("/", async ([FromBody] RatingDTO rating, IRatingApiHandler handler, CancellationToken cancellationToken) =>
            await handler.UpdateAsync(rating, cancellationToken)
        ).RequireAuthorization();
    }
}

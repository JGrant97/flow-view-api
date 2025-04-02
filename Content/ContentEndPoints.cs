using flow_view.Ratings;
using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;

namespace flow_view.Content;

public static class ContentEndPoints
{
    public static void MapContentEndpoints(this WebApplication builder)
    {
        var api = builder.MapGroup("/content").WithTags("Content");

        api.MapGet("/", async (IContentApiHandler handler, CancellationToken cancellationToken) =>
            await handler.GetAsync(cancellationToken)
        );

        api.MapGet("/{id}", async (Guid id, IContentApiHandler handler, CancellationToken cancellationToken) =>
            await handler.GetAsync(id, cancellationToken)
        );

        api.MapDelete("/{id}", async (Guid id, IContentApiHandler handler, CancellationToken cancellationToken) =>
            await handler.DeleteAsync(id, cancellationToken)
        )
        .RequireAuthorization();

        api.MapPost("/", async ([FromForm] ContentDTO content, IContentApiHandler handler, CancellationToken cancellationToken) =>
            await handler.CreateAsync(content, cancellationToken)
        )
        .RequireAuthorization();

        api.MapPut("/", async ([FromForm] ContentDTO content, IContentApiHandler handler, CancellationToken cancellationToken) =>
            await handler.UpdateAsync(content, cancellationToken)
        )
        .RequireAuthorization();

        api.MapPost("/filter", async (ContentFilterRequest request, IContentApiHandler handler, IRatingRepository ratingRepository, CancellationToken cancellationToken) =>
            await handler.FilterAsync(request, cancellationToken)
        );
    }
}

using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http.HttpResults;

namespace flow_view.Content;

public interface IContentApiHandler
{
    Task<Results<Ok<List<ContentDTO>>, NotFound<string>>> GetAsync(CancellationToken cancellationToken);
    Task<Results<Ok<List<ContentPreviewDTO>>, NotFound<string>>> FilterAsync(ContentFilterRequest request, CancellationToken cancellationToken);
    Task<Results<Ok<ContentDTO>, NotFound<string>>> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Ok<ContentDTO>> CreateAsync(ContentDTO content, CancellationToken cancellationToken);
    Task<Ok<ContentDTO>> UpdateAsync(ContentDTO content, CancellationToken cancellationToken);
    Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

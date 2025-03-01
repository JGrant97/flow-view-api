using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http.HttpResults;

namespace flow_view.Content;

public interface IContentHelper
{
    Task<Results<Ok<List<ContentDTO>>, NotFound<string>>> GetAsync(IContentRepository contentRepository);
    Task<Results<Ok<ContentDTO>, NotFound<string>>> GetAsync(Guid id, IContentRepository contentRepository);
    Task<Ok<ContentDTO>> CreateAsync(ContentDTO content, IContentRepository contentRepository);
    Task<Ok<ContentDTO>> UpdateAsync(ContentDTO content, IContentRepository contentRepository);
    Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, IContentRepository contentRepository);
}

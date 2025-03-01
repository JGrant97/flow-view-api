using flow_view_database.Content;
using Microsoft.AspNetCore.Http.HttpResults;

namespace flow_view.Content;

public class ContentHelper : IContentHelper
{
    public Task<Ok<ContentDTO>> CreateAsync(ContentDTO content, IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }

    public Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }

    public Task<Results<Ok<List<ContentDTO>>, NotFound<string>>> GetAsync(IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }

    public Task<Results<Ok<ContentDTO>, NotFound<string>>> GetAsync(Guid id, IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }

    public Task<Ok<ContentDTO>> UpdateAsync(ContentDTO content, IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }
}

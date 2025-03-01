using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace flow_view.Content;

public class ContentHelper : IContentHelper
{
    public async Task<Ok<ContentDTO>> CreateAsync(ContentDTO content, IContentRepository contentRepository)
    {
        //Upload to cloud and set file path for video and thumbnail
        content.FilePath = "dsadas";
        content.Thumbnail = "aaaaa";

        return TypedResults.Ok((await contentRepository.CreateAsync(content.MapToDBModel())).MapToDTO());
    }

    public async Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, IContentRepository contentRepository)
    {
        try
        {
            await contentRepository.DeleteAsync(id);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<List<ContentDTO>>, NotFound<string>>> GetAsync(IContentRepository contentRepository)
    {
        var results = await contentRepository.Get().ToListAsync();

        if (results is null || results.Count == 0)
            return TypedResults.NotFound("No content found.");

        return TypedResults.Ok(results.MapToDTO());
    }

    public async Task<Results<Ok<ContentDTO>, NotFound<string>>> GetAsync(Guid id, IContentRepository contentRepository)
    {
        try
        {
            var result = await contentRepository.GetAsync(id);
            return TypedResults.Ok(result.MapToDTO());
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public Task<Ok<ContentDTO>> UpdateAsync(ContentDTO content, IContentRepository contentRepository)
    {
        throw new NotImplementedException();
    }
}

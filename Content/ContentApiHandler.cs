using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace flow_view.Content;

public class ContentApiHandler(IContentRepository contentRepo) : IContentApiHandler
{
    public async Task<Ok<ContentDTO>> CreateAsync(ContentDTO content,  CancellationToken cancellationToken)
    {
        //Upload to cloud and set file path for video and thumbnail
        var entity = content.MapToDBModel();
        entity.FilePath = "dsadas";
        entity.Thumbnail = "aaaaa";

        return TypedResults.Ok((await contentRepo.CreateAsync(entity, cancellationToken)).MapToDTO());
    }

    public async Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id,  CancellationToken cancellationToken)
    {
        try
        {
            await contentRepo.DeleteAsync(id, cancellationToken);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<List<ContentDTO>>, NotFound<string>>> GetAsync( CancellationToken cancellationToken)
    {
        var results = await contentRepo.Get().ToListAsync();

        if (results is null || results.Count == 0)
            return TypedResults.NotFound("No content found.");

        return TypedResults.Ok(results.MapToDTO());
    }

    public async Task<Results<Ok<ContentDTO>, NotFound<string>>> GetAsync(Guid id,  CancellationToken cancellationToken)
    {
        try
        {
            var result = await contentRepo.GetAsync(id, cancellationToken);

            if(result is null)
                return TypedResults.NotFound("No content found.");

            return TypedResults.Ok(result.MapToDTO());
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    public async Task<Results<Ok<List<ContentPreviewDTO>>, NotFound<string>>> FilterAsync(ContentFilterRequest request,  CancellationToken cancellationToken)
    {
        var result = await contentRepo.FilterAsync(request, cancellationToken);

        if (result is null)
            return TypedResults.NotFound("No content found.");

        return TypedResults.Ok(result);
    }

    public Task<Ok<ContentDTO>> UpdateAsync(ContentDTO content,  CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

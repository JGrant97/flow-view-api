﻿using flow_view.Ratings;
using flow_view_database.Content;
using flow_view_database.Rating;
using Microsoft.AspNetCore.Mvc;

namespace flow_view.Content;

public static class ContentEndPoints
{
    public static void MapContentEndpoints(this WebApplication app)
    {
        app.MapGet("/content", async (IContentHelper contentHelper, IContentRepository contentRepository) =>
            await contentHelper.GetAsync(contentRepository)
        ).WithTags("Content");

        app.MapGet("/content/{id}", async (Guid id, IContentHelper contentHelper, IContentRepository contentRepository) =>
            await contentHelper.GetAsync(id, contentRepository)
        ).WithTags("Content");

        app.MapDelete("/ratcontenting/delete/{id}", async (Guid id, IContentHelper contentHelper, IContentRepository contentRepository) =>
            await contentHelper.DeleteAsync(id, contentRepository)
        ).WithTags("Content");

        app.MapPost("/content/create", async ([FromForm] ContentDTO content, IContentHelper contentHelper, IContentRepository contentRepository) =>
            await contentHelper.CreateAsync(content, contentRepository)
        ).WithTags("Content");

        app.MapPut("/content/update", async ([FromForm] ContentDTO content, IContentHelper contentHelper, IContentRepository contentRepository) =>
            await contentHelper.UpdateAsync(content, contentRepository)
        ).WithTags("Content");
    }
}

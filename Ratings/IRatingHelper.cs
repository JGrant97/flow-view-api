using flow_view_database.Rating;
using Microsoft.AspNetCore.Http.HttpResults;

namespace flow_view.Ratings;

public interface IRatingHelper
{
    Task<Results<Ok<List<RatingDTO>>, NotFound<string>>> GetAsync(IRatingRepository ratingRepository);
    Task<Results<Ok<RatingDTO>, NotFound<string>>> GetAsync(Guid id, IRatingRepository ratingRepository);
    Task<Ok<RatingDTO>> CreateAsync(RatingDTO rating, IRatingRepository ratingRepository);
    Task<Ok<RatingDTO>> UpdateAsync(RatingDTO rating, IRatingRepository ratingRepository);
    Task<Results<Ok, NotFound<string>>> DeleteAsync(Guid id, IRatingRepository ratingRepository);
}

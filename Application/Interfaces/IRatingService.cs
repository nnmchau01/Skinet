using Application.Models.Rating;

namespace Application.Interfaces;

public interface IRatingService
{
    Task<List<RatingDetailModel>> GetListRating();
    Task<bool> UpdateRating(RatingUpdateModel model);
    Task<int> GetRatingDetail(RatingFilterModel model);
    Task<double> GetAverageStarPoint(RatingFilterModel model);
}
using Application.Interfaces;
using Application.Models.Rating;
using Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RatingService : IRatingService
{
    private readonly IApplicationDbContext _dbContext;

    public RatingService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RatingDetailModel>> GetListRating()
    {
        var ratings = await _dbContext.Ratings.Where(s => !s.IsDeleted).ToListAsync();
        var products = await _dbContext.Products.Where(s => !s.IsDeleted).ToListAsync();
        var brands = await _dbContext.Brands.Where(s => !s.IsDeleted).ToListAsync();
        var categories = await _dbContext.Categories.Where(s => !s.IsDeleted).ToListAsync();

        var averageRatings = ratings.GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                AverageRatingStart = g.Average(r => r.StarPoint)
            });

        var data = from p in products
            join ave in averageRatings on p.Id equals ave.ProductId into avgRatings
            from rating in avgRatings.DefaultIfEmpty()
            join b in brands on p.BrandId equals b.Id
            join c in categories on p.CategoryId equals c.Id
            select new
            {
                ProductName = p.Name,
                Url = p.Url,
                Brand = b.Name,
                Category = c.Name,
                DefaultImage = p.DefaultImage,
                AverageRatingStart = rating?.AverageRatingStart,
                ProductId = p.Id
            };
        
        return data.Adapt<List<RatingDetailModel>>();
    }

    public async Task<bool> UpdateRating(RatingUpdateModel model)
    {
        try
        {
            var currentRating = await _dbContext.Ratings.FirstOrDefaultAsync(s => s.ProductId == model.ProductId && s.UserId == model.UserId);

            if (currentRating == null)
            {
                await _dbContext.Ratings.AddAsync(new Rating()
                {
                    ProductId = model.ProductId,
                    UserId = model.UserId,
                    StarPoint = model.StarPoint
                });

                await _dbContext.SaveChangesAsync(new CancellationToken());
            }
            else
            {
                currentRating.StarPoint = model.StarPoint;

                _dbContext.Ratings.Update(currentRating);

                await _dbContext.SaveChangesAsync(new CancellationToken());
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }

    public async Task<int> GetRatingDetail(RatingFilterModel model)
    {
        var currentRating = await _dbContext.Ratings.FirstOrDefaultAsync(s => s.ProductId == model.ProductId && s.UserId == model.UserId);

        return currentRating?.StarPoint ?? 0;
    }

    public async Task<double> GetAverageStarPoint(RatingFilterModel model)
    {
        try
        {
            var averageStarPoint = await _dbContext.Ratings
                .Where(r => r.ProductId == model.ProductId)
                .AverageAsync(r => r.StarPoint);

            return averageStarPoint;
        }
        catch (Exception e)
        {
            return 0;
        }
    }
}
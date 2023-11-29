using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Brands;
using Domain.Entities;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BrandService : IBrandService
{
    private readonly IApplicationDbContext _dbContext;

    public BrandService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BrandDetailModel>> GetListBrand()
    {
        var data = await _dbContext.Brands
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return data.Adapt<List<BrandDetailModel>>();
    }

    public async Task<BrandDetailModel> GetDetail(string id)
    {
        var data = await _dbContext.Brands
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(id));

        return data == null ? new BrandDetailModel() : data.Adapt<BrandDetailModel>();
    }

    public async Task<bool> Delete(DeleteModel model)
    {
        var data = await _dbContext.Brands
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data == null)
        {
            return false;
        }

        data.IsDeleted = true;

        _dbContext.Brands.Update(data);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<bool> AddNew(AddNewBrandModel model)
    {
        var data = await _dbContext.Brands
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data != null)
        {
            return false;
        }

        var newData = model.Adapt<Brand>();
        newData.Code = model.Name.VietnameseToNormalString();

        await _dbContext.Brands.AddAsync(newData);

        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<bool> Update(UpdateBrandModel model)
    {
        var data = await _dbContext.Brands
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data == null)
        {
            return false;
        }

        data.Name = model.Name;
        data.Code = model.Name.VietnameseToNormalString();

        _dbContext.Brands.Update(data);

        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }
}
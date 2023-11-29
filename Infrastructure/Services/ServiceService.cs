using Application.Interfaces;
using Application.Models.Rating;
using Application.Models.Service;
using Domain.Entities;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ServiceService : IServiceService
{
    private readonly IApplicationDbContext _dbContext;

    public ServiceService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<ServiceDetailModel>> GetList()
    {
        var data = await _dbContext.Services
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return data.Adapt<List<ServiceDetailModel>>();
    }

    public async Task<bool> Add(AddNewServiceModel model)
    {
        var newData = model.Adapt<Service>();
        newData.ServiceCode = model.ServiceName.VietnameseToNormalString();

        await _dbContext.Services.AddAsync(newData);

        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<bool> Update(UpdateServiceModel model)
    {
        var data = await _dbContext.Services
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data == null)
        {
            return false;
        }

        data.ServiceName = model.ServiceName;
        data.ServiceCode = model.ServiceName.VietnameseToNormalString();
        data.Price = model.Price;
        
        _dbContext.Services.Update(data);

        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<bool> Delete(string serviceId)
    {
        var data = await _dbContext.Services
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(serviceId));

        if (data == null)
        {
            return false;
        }

        data.IsDeleted = true;
        _dbContext.Services.Update(data);

        await _dbContext.SaveChangesAsync(new CancellationToken());
        
        return true;
    }

    public async Task<ServiceDetailModel> GetDetail(string serviceId)
    {
        var data = await _dbContext.Services
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(serviceId));

        return data == null ? new ServiceDetailModel() : data.Adapt<ServiceDetailModel>();
    }
}
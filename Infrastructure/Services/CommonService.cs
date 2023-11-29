using Application.Interfaces;
using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Menu;
using Application.Models.Status;
using Domain.Constants;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CommonService : ICommonService
{
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly IApplicationDbContext _dbContext;

    public CommonService(IBrandService brandService
        , ICategoryService categoryService
        , IApplicationDbContext dbContext)
    {
        _brandService = brandService;
        _categoryService = categoryService;
        _dbContext = dbContext;
    }

    public async Task<MenuModel> GetMenu(bool isAdmin)
    {
        var categories = await _categoryService.GetListCategory();
        var brands = await _brandService.GetListBrand();

        var menu = new MenuModel()
        {
            Brands = brands,
            Categories = categories,
            IsShowButtonAdmin = isAdmin
        };

        return menu;
    }

    public async Task<List<StatusDetailModel>> GetDropdownStatus(string type)
    {
        var status = await _dbContext.Status.Where(s => !s.IsDeleted && s.Type == type).ToListAsync();

        return status.Adapt<List<StatusDetailModel>>();
    }
}
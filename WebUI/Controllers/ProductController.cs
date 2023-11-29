using System.Globalization;
using Application.Interfaces;
using Application.Models.Product;
using Application.Models.Search;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ProductController : BaseController
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;

    public ProductController(IProductService productService
        , ICategoryService categoryService
        , IBrandService brandService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _brandService = brandService;
    }

    public async Task<IActionResult> Detail(string id)
    {
        var data = await _productService.Detail(id);

        if (string.IsNullOrEmpty(data?.Name))
        {
            return Redirect("/Common/NotFoundPage");
        }

        return View(data);
    }
    
    [Route("/p/{url}")]
    public async Task<IActionResult> DetailByUrl(string url)
    {
        var data = await _productService.DetailByUrl(url);

        if (string.IsNullOrEmpty(data?.Name))
        {
            return Redirect("/Common/NotFoundPage");
        }

        return View("Detail", data);
    }

    public async Task<IActionResult> Brand(string value = "", int page = 1)
    {
        var model = new SearchPageModel();
        var dataList = await _productService.GetListCategoryOrBrand("brand", value);
        var minPrice = dataList?.Count > 0 ? dataList.Min(r => r.Price) : 0;
        var maxPrice = dataList?.Count > 0 ? dataList.Max(r => r.Price) : 0;
        model.PagingResult = PagingHelper<ProductDetailModel>.ToPaging(dataList, page, 10, value, minPrice, maxPrice);
        model.Categories = await _categoryService.GetListCategory();
        model.Brands = await _brandService.GetListBrand();

        return View(model);
    }

    public async Task<IActionResult> Category(string value = "", int page = 1)
    {
        var model = new SearchPageModel();
        var dataList = await _productService.GetListCategoryOrBrand("category", value);
        var minPrice = dataList?.Count > 0 ? dataList.Min(r => r.Price) : 0;
        var maxPrice = dataList?.Count > 0 ? dataList.Max(r => r.Price) : 0;
        model.PagingResult = PagingHelper<ProductDetailModel>.ToPaging(dataList, page, 10, value, minPrice, maxPrice);
        model.Categories = await _categoryService.GetListCategory();
        model.Brands = await _brandService.GetListBrand();

        return View(model);
    }

    public async Task<IActionResult> Search(string searchText, int page = 1)
    {
        var model = new SearchPageModel();
        var dataList = await _productService.GetListSearch(searchText);
        var minPrice = dataList?.Count > 0 ? dataList.Min(r => r.Price) : 0;
        var maxPrice = dataList?.Count > 0 ? dataList.Max(r => r.Price) : 0;
        model.PagingResult =
            PagingHelper<ProductDetailModel>.ToPaging(dataList, page, 10, searchText, minPrice, maxPrice);
        model.Categories = await _categoryService.GetListCategory();
        model.Brands = await _brandService.GetListBrand();

        return View(model);
    }

    public async Task<IActionResult> SearchRangePrice(decimal? min, decimal? max, int page = 1)
    {
        var model = new SearchPageModel();
        min ??= 0;
        max ??= int.MaxValue;
        var dataListMinMax = await _productService.GetListSearch("");
        var dataList = _productService.GetListRangePrice(dataListMinMax, min, max);
        var minPrice = dataList?.Count > 0 ? dataList.Min(r => r.Price) : 0;
        var maxPrice = dataList?.Count > 0 ? dataList.Max(r => r.Price) : 0;
        model.PagingResult = PagingHelper<ProductDetailModel>.ToPaging(dataList, page, 10, "", minPrice, maxPrice);
        model.Categories = await _categoryService.GetListCategory();
        model.Brands = await _brandService.GetListBrand();

        return View(model);
    }
}
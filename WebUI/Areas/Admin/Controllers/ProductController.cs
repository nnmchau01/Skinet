using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Property;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class ProductController : BaseAdminController
{
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProductService _productService;

    public ProductController(ICategoryService categoryService
        , IBrandService brandService
        , IWebHostEnvironment webHostEnvironment
        , IProductService productService)
    {
        _categoryService = categoryService;
        _brandService = brandService;
        _webHostEnvironment = webHostEnvironment;
        _productService = productService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _productService.GetListProduct();

        return PartialView("_ProductListPartial", data);
    }

    [HttpPost]
    public IActionResult AddNewRowProperty([FromBody] List<PropertyDetailModel> request)
    {
        request.Add(new PropertyDetailModel()
        {
            ProductId = request?.Count > 0 ? request.First().ProductId : null,
            Name = "",
            Value = ""
        });
        return PartialView("_AddNewRowPropertyPartial", request);
    }

    public async Task<IActionResult> Create()
    {
        var brands = await _brandService.GetListBrand();
        var categories = await _categoryService.GetListCategory();

        var modelFilter = new ProductFilterModel()
        {
            Brands = brands,
            Categories = categories
        };

        return View("AddNewProduct", modelFilter);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitCreate([FromForm] AddNewProductModel request)
    {
        var imgs = new List<ImageDetailModel>();

        var contentRootPath = _webHostEnvironment.WebRootPath;

        foreach (var formFile in request.Images.Where(formFile => formFile.Length > 0))
        {
            var imageId = Guid.NewGuid();
            var localPath = $"img/product/{imageId.ToString()}.png";
            var filePath = Path.Combine(contentRootPath, localPath);

            await using var stream = System.IO.File.Create(filePath);
            await formFile.CopyToAsync(stream);

            imgs.Add(new ImageDetailModel()
            {
                Id = imageId,
                ProductId = request.Id,
                LocalLinkImage = $"/{localPath}"
            });
        }

        request.ListImage = imgs;

        var success = await _productService.AddNew(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _productService.Delete(request.Id);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    public async Task<IActionResult> Update(string id)
    {
        var brands = await _brandService.GetListBrand();
        var categories = await _categoryService.GetListCategory();
        var productDetail = await _productService.Detail(id);

        if (string.IsNullOrEmpty(productDetail?.Name))
        {
            return Redirect("/Common/NotFoundPage");
        }

        var modelFilter = new ProductFilterModel()
        {
            Brands = brands,
            Categories = categories,
            Product = productDetail
        };

        return View("UpdateProduct", modelFilter);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromForm] UpdateProductModel request)
    {
        var imgs = new List<ImageDetailModel>();

        var contentRootPath = _webHostEnvironment.WebRootPath;

        if (request.Images?.Count > 0)
        {
            foreach (var formFile in request.Images.Where(formFile => formFile.Length > 0))
            {
                var imageId = Guid.NewGuid();
                var localPath = $"img/product/{imageId.ToString()}.png";
                var filePath = Path.Combine(contentRootPath, localPath);

                await using var stream = System.IO.File.Create(filePath);
                await formFile.CopyToAsync(stream);

                imgs.Add(new ImageDetailModel()
                {
                    Id = imageId,
                    ProductId = request.Id,
                    LocalLinkImage = $"/{localPath}"
                });
            }
        }

        request.ListImage = imgs;

        var success = await _productService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
}
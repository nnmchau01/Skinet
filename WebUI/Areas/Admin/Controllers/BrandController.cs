using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Property;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class BrandController : BaseAdminController
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _brandService.GetListBrand();

        return PartialView("_BrandListPartial", data);
    }

    // GET
    public IActionResult AddNewPartial()
    {
        return PartialView("_AddNewPartial");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAddNew([FromBody] AddNewBrandModel request)
    {
        var success = await _brandService.AddNew(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _brandService.Delete(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    // GET
    public async Task<IActionResult> UpdatePartial(string id)
    {
        var data = await _brandService.GetDetail(id);

        return PartialView("_UpdatePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromBody] UpdateBrandModel request)
    {
        var success = await _brandService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
}
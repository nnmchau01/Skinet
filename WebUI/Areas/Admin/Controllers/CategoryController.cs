using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Property;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class CategoryController : BaseAdminController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _categoryService.GetListCategory();

        return PartialView("_CategoryListPartial", data);
    }

    // GET
    public IActionResult AddNewPartial()
    {
        return PartialView("_AddNewPartial");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAddNew([FromBody] AddNewCategoryModel request)
    {
        var success = await _categoryService.AddNew(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _categoryService.Delete(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    // GET
    public async Task<IActionResult> UpdatePartial(string id)
    {
        var data = await _categoryService.GetDetail(id);

        return PartialView("_UpdatePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromBody] UpdateCategoryModel request)
    {
        var success = await _categoryService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
}
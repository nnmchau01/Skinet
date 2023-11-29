using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Property;
using Application.Models.Service;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class ServiceController : BaseAdminController
{
    private readonly ICategoryService _categoryService;
    private readonly IServiceService _serviceService;

    public ServiceController(ICategoryService categoryService, IServiceService serviceService)
    {
        _categoryService = categoryService;
        _serviceService = serviceService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _serviceService.GetList();

        return PartialView("_ServiceListPartial", data);
    }

    // GET
    public IActionResult AddNewPartial()
    {
        return PartialView("_AddNewPartial");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAddNew([FromBody] AddNewServiceModel request)
    {
        var success = await _serviceService.Add(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _serviceService.Delete(request.Id.ToString());

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    // GET
    public async Task<IActionResult> UpdatePartial(string id)
    {
        var data = await _serviceService.GetDetail(id);

        return PartialView("_UpdatePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromBody] UpdateServiceModel request)
    {
        var success = await _serviceService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
}
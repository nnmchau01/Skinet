using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Promotion;
using Application.Models.Property;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class PromotionController : BaseAdminController
{
    private readonly IPromotionService _promotionService;

    public PromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _promotionService.GetListPromotion();

        return PartialView("_PromotionListPartial", data);
    }

    // GET
    public IActionResult AddNewPartial()
    {
        return PartialView("_AddNewPartial");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAddNew([FromBody] AddNewPromotionModel request)
    {
        var success = await _promotionService.AddNew(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _promotionService.Delete(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    // GET
    public async Task<IActionResult> UpdatePartial(string id)
    {
        var data = await _promotionService.GetDetail(id);

        return PartialView("_UpdatePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromBody] UpdatePromotionModel request)
    {
        var success = await _promotionService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
}
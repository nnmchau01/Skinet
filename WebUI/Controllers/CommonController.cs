using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class CommonController : BaseController
{
    private readonly IInitDataService _initDataService;
    private readonly IIdentityService _identityService;
    private readonly IPromotionService _promotionService;
    private readonly ICommonService _commonService;
    private readonly IOrderService _orderService;

    public CommonController(IInitDataService initDataService
        , IIdentityService identityService
        , ICommonService commonService
        , IPromotionService promotionService
        , IOrderService orderService)
    {
        _initDataService = initDataService;
        _identityService = identityService;
        _commonService = commonService;
        _promotionService = promotionService;
        _orderService = orderService;
    }

    public async Task<IActionResult> InitData()
    {
        await _initDataService.InitData();

        return View("InitData");
    }

    [HttpGet]
    public IActionResult GetHeader()
    {
        var user = _identityService.GetCurrentUserLogin();

        return PartialView("_GetHeaderPartial", user);
    }

    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var data = await _commonService.GetMenu(_identityService.IsInRoleAsync(SecurityRoles.Admin));
        return PartialView("_MenuPartial", data);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }

    [HttpGet]
    public IActionResult AccountDeleted()
    {
        return View("AccountDeleted");
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View("Error");
    }

    [HttpGet]
    public IActionResult NotFoundPage()
    {
        return View("NotFound");
    }

    [HttpGet]
    public async Task<IActionResult> GetPromotion(string code)
    {
        var data = await _promotionService.GetDetailByCode(code);
        return Json(data);
    }

    // GET
    public async Task<IActionResult> GetOrderDetailPartial(string code)
    {
        var data = await _orderService.DetailByCode(code);

        return PartialView("_OrderDetailPartial", data);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPromotionList()
    {
        var data = await _promotionService.GetListPromotion();
        
        return PartialView("_PromotionListPartial", data);
    }
}
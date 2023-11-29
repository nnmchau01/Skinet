using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class TestController : BaseController
{
    private readonly IInitDataService _initDataService;
    private readonly IIdentityService _identityService;
    private readonly ICategoryService _categoryService;
    private readonly IPromotionService _promotionService;
    private readonly ICommonService _commonService;
    private readonly IOrderService _orderService;

    public TestController(IInitDataService initDataService
        , IIdentityService identityService
        , ICategoryService categoryService
        , ICommonService commonService
        , IPromotionService promotionService
        , IOrderService orderService)
    {
        _initDataService = initDataService;
        _identityService = identityService;
        _categoryService = categoryService;
        _commonService = commonService;
        _promotionService = promotionService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _orderService.Detail("5BC46071-340D-4AA7-B833-FE51ADB80929");

        return Json(data);
    }
}
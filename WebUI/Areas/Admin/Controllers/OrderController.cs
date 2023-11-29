using Application.Interfaces;
using Application.Models.Order;
using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class OrderController : BaseAdminController
{
    private readonly IOrderService _orderService;
    private readonly ICommonService _commonService;

    public OrderController(ICommonService commonService, IOrderService orderService)
    {
        _orderService = orderService;
        _commonService = commonService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _orderService.GetListOrder();

        return PartialView("_OrderListPartial", data);
    }

    // GET
    public async Task<IActionResult> DetailPartial(string id)
    {
        var data = await _orderService.Detail(id);

        return PartialView("_DetailPartial", data);
    }

    public async Task<IActionResult> UpdatePartial(string id)
    {
        var listOrderStatus = await _commonService.GetDropdownStatus(StatusType.Order.ReadDescription());
        var listPaymentStatus = await _commonService.GetDropdownStatus(StatusType.Payment.ReadDescription());
        var orderDetail = await _orderService.Detail(id);

        var modelFilter = new OrderFilterModel()
        {
            OrderDetail = orderDetail,
            ListOrderStatus = listOrderStatus,
            ListPaymentStatus = listPaymentStatus
        };

        if (string.IsNullOrEmpty(orderDetail?.Code))
        {
            return NotFound();
        }

        return View("_UpdatePartial", modelFilter);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdate([FromBody] UpdateStatusOrderModel request)
    {
        var success = await _orderService.Update(request);

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetListOrderReport([FromQuery] ChartReportSummaryFilterModel request)
    {
        var data = await _orderService.GenerateOrderReport(request.Year, request.Month);

        return Json(data);
    }
}
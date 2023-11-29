using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class BookingController : BaseAdminController
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _bookingService.GetList();

        return PartialView("_BookingListPartial", data);
    }
    
    public async Task<IActionResult> GetDetailPartial(string id)
    {
        var data = await _bookingService.GetDetail(id);

        return PartialView("_DetailPartial", data);
    }
}
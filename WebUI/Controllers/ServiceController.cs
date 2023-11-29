using Application.Interfaces;
using Application.Models.Booking;
using Application.Models.Service;
using Domain.Constants;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace WebUI.Controllers;

public class ServiceController : BaseController
{
    private readonly IServiceService _serviceService;
    private readonly IBookingService _bookingService;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public ServiceController(IServiceService serviceService, IBookingService bookingService, IEmailService emailService,
        UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _serviceService = serviceService;
        _bookingService = bookingService;
        _emailService = emailService;
        _userManager = userManager;
        _identityService = identityService;
    }

    
    public async Task<IActionResult> Index()
    {
        var data = await _serviceService.GetList();

        return View(data);
    }


    //[ValidateRecaptcha]
    [HttpPost]
    public async Task<IActionResult> Book([FromBody] AddBookingServiceModel request)
    {
        
        var success = await _bookingService.Add(request);

        if (success)
        {
            var service = await _serviceService.GetDetail(request.ServiceId);
            
            var usersWithAdminRole = await _userManager.GetUsersInRoleAsync(SecurityRoles.Admin);

            // var emailUser = await _userManager.FindByIdAsync(_identityService.GetCurrentUserLogin().Id);
            var serviceName = service.ServiceName;
            var price = service.Price.FormatMoney();

            foreach (var user in usersWithAdminRole)
            {
                await _emailService.SendNewBookingAsync(user.Email, "", request.FullName,
                    request.Time + " " + request.Date, serviceName, price, request.PhoneNumber);
            }
            
            await _emailService.SendNewBookingAsync("", request.Email, request.FullName,
                request.Time + " " + request.Date, serviceName, price, request.PhoneNumber);
        }
        
        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    [HttpGet]
    public async Task<IActionResult> GetListFilter()
    {
        var data = await _serviceService.GetList();

        var newListReturn = data.Select(s => new
        {
            id = s.Id,
            text = s.ServiceName + $" ({s.Price.FormatMoney()}đ)"
        });

        return new OkObjectResult(newListReturn);
    }

    [HttpGet]
    public IActionResult GetListAvailableTime()
    {
        var listTimes = new List<object>();

        var startTime = DateTime.ParseExact("08:00", "HH:mm", null);
        var endTime = DateTime.ParseExact("21:00", "HH:mm", null);

        var interval = TimeSpan.FromMinutes(15);
        var currentTime = startTime;

        while (currentTime <= endTime)
        {
            listTimes.Add(new { id = currentTime.ToString("HH:mm"), text = currentTime.ToString("HH:mm") });
            currentTime = currentTime.Add(interval);
        }

        return new OkObjectResult(listTimes);
    }
}
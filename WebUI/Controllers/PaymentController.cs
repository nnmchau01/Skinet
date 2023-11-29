using Application.Interfaces;
using Application.Models.Checkout;
using Application.Models.Order;
using Application.Models.Payment;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class PaymentController : BaseController
{
    private readonly IVNPayService _vnPayService;
    private readonly IPayPalService _payPalService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IMomoService _momoService;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public PaymentController(IVNPayService vnPayService, IPayPalService payPalService, ICartService cartService, IOrderService orderService, IMomoService momoService, IEmailService emailService,
        UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _vnPayService = vnPayService;
        _payPalService = payPalService;
        _cartService = cartService;
        _orderService = orderService;
        _momoService = momoService;
        _emailService = emailService;
        _userManager = userManager;
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutModel request)
    {
        var paymentCode = Guid.NewGuid().ToString().Split("-")[0];
        var carts = await _cartService.GetListCart(request.Carts);

        if (!(carts?.Count > 0)) return BadRequestResponse("Cart is empty");

        switch (request.PaymentMethod.ToLower())
        {
            case "vnpay":
                var responseUriVnPay = _vnPayService.CreatePayment(new PaymentInfoModel()
                {
                    TotalAmount = (double)carts.First().TotalPriceAll,
                    PaymentCode = paymentCode
                }, HttpContext);
                return SuccessResponse(responseUriVnPay.Uri);
            case "paypal":
                var responseUriPayPal = await _payPalService.CreatePayment(new PaymentInfoModel()
                {
                    TotalAmount = (double)carts.First().TotalPriceAll,
                    PaymentCode = paymentCode
                });
                return SuccessResponse(responseUriPayPal.Uri);
            case "momo":
                var responseUriMomo = await _momoService.CreatePayment(new PaymentInfoModel()
                {
                    TotalAmount = (double)carts.First().TotalPriceAll,
                    PaymentCode = paymentCode
                });
                return SuccessResponse(responseUriMomo.Uri);
            case "cash":
                return SuccessResponse($"/Home/PaymentCallback?success=true&paymentMethod=Cash");
            default:
                return BadRequestResponse("Invalid payment method");
        }
    }

    public async Task<IActionResult> PaymentCallback()
    {
        var queryCollection = Request.Query;

        if (queryCollection?.Count == 0 || queryCollection == null)
            return Redirect("/Home/PaymentCallback?success=false");

        var paymentMethod = queryCollection.FirstOrDefault(t => t.Key.Contains("payment_method")).Value
            .ToString().ToLower();

        switch (paymentMethod)
        {
            case "vnpay":
                var vnPayResponse = _vnPayService.PaymentExecute(Request.Query);
                return Redirect(!vnPayResponse.Success
                    ? "/Home/PaymentCallback?success=false&paymentMethod=VnPay"
                    : "/Home/PaymentCallback?success=true&paymentMethod=VnPay");
            case "paypal":
                var payPalResponse = await _payPalService.PaymentExecute(Request.Query);
                return Redirect(!payPalResponse.Success
                    ? "/Home/PaymentCallback?success=false&paymentMethod=PayPal"
                    : "/Home/PaymentCallback?success=true&paymentMethod=PayPal");
            case "momo":
                var momoResponse = _momoService.PaymentExecute(Request.Query);
                return Redirect(!momoResponse.Success
                    ? "/Home/PaymentCallback?success=false&paymentMethod=MoMo"
                    : "/Home/PaymentCallback?success=true&paymentMethod=MoMo");
            default:
                return Redirect("/");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CompleteOrder([FromBody] AddNewOrderModel request)
    {
        var carts = await _cartService.GetListCart(request.Carts);
        request.Carts = carts;
        var data = await _orderService.AddNew(request);
        var order = await _orderService.DetailByCode(data);

        var usersWithAdminRole = await _userManager.GetUsersInRoleAsync(SecurityRoles.Admin);

        // var emailUser = await _userManager.FindByIdAsync(_identityService.GetCurrentUserLogin().Id);
        

        //foreach (var user in usersWithAdminRole)
        //{
        //    await _emailService.SendNewOrderAsync(user.Email, "", order.Code, order.CustomerName,
        //        order.Address, order.PhoneNumber);
        //}


        await _emailService.SendNewOrderAsync("", order.Note, order.Code, order.CustomerName,
                order.Address, order.PhoneNumber);

        return Ok(data);
        

    }
}
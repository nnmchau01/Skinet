using Application.Common.Models;
using Application.Models.Payment;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IVNPayService
{
    ResponseUriModel CreatePayment(PaymentInfoModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collection);
}
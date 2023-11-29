using Application.Common.Models;
using Application.Models.Payment;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IMomoService
{
    Task<ResponseUriModel> CreatePayment(PaymentInfoModel dto);

    PaymentResponseModel PaymentExecute(IQueryCollection collection);
}
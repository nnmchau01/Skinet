using Application.Common.Models;
using Application.Models.Brands;
using Application.Models.Cart;
using Application.Models.Categories;
using Application.Models.Payment;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPayPalService
{
    Task<ResponseUriModel> CreatePayment(PaymentInfoModel dto);

    Task<PaymentResponseModel> PaymentExecute(IQueryCollection collection);
}
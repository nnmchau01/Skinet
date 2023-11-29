using System.Net;
using Application.Common.Models;
using Application.Common.Options;
using Application.Interfaces;
using Application.Models.Payment;
using Application.Models.PayPal;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using RestSharp;
using RestSharp.Authenticators;
using Payment = PayPal.v1.Payments.Payment;

namespace Infrastructure.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private const double ExchangeRate = 22_863.0;
        private readonly IOptions<PaypalOption> _options;

        public PayPalService(IConfiguration configuration, IOptions<PaypalOption> options)
        {
            _configuration = configuration;
            _options = options;
        }

        public static double ConvertVndToDollar(double vnd)
        {
            var total = Math.Round(vnd / ExchangeRate, 2);

            return total;
        }

        public async Task<ResponseUriModel> CreatePayment(PaymentInfoModel model)
        {
            // var envProd = new LiveEnvironment(_configuration["PaypalProduction:ClientId"],
            //     _configuration["PaypalProduction:SecretKey"]);

            var envSandbox =
                new SandboxEnvironment(_options.Value.ClientId, _options.Value.SecretKey);
            var client = new PayPalHttpClient(envSandbox);
            var paypalOrderId = DateTime.Now.Ticks;
            var urlCallBack = _configuration["PaymentConfig:ReturnUrl"];
            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = ConvertVndToDollar(model.TotalAmount).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = ConvertVndToDollar(model.TotalAmount).ToString(),
                            }
                        },
                        ItemList = new ItemList()
                        {
                            Items = new List<Item>()
                            {
                                new Item()
                                {
                                    Name = " | New Order",
                                    Currency = "USD",
                                    Price = ConvertVndToDollar(model.TotalAmount).ToString(),
                                    Quantity = 1.ToString(),
                                    Sku = "sku",
                                    Tax = "0",
                                    Url = "https://www.progcoder.com" // Url detail of Item
                                }
                            }
                        },
                        Description = $"Invoice #{model.PaymentCode}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    CancelUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return new ResponseUriModel()
                {
                    Name = "PayPal",
                    Uri = paymentUrl
                };

            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }

            return new ResponseUriModel()
            {
                Name = "PayPal",
                Uri = paymentUrl
            };
        }

        public async Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections)
        {
            var response = new PaymentResponseModel();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("transaction_id"))
                {
                    response.TransactionId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_id"))
                {
                    response.OrderId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    response.PaymentMethod = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("success"))
                {
                    response.Success = Convert.ToInt32(value) > 0;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("paymentid"))
                {
                    response.PaymentId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payerid"))
                {
                    response.PayerId = value;
                }
            }

            var accessToken = await GetAccessToken();
            var resExecute = await ExecutePayment(_options.Value.BaseUrl, accessToken, response.PaymentId, response.PayerId);
            
            return response;
        }

        private async Task<string> GetAccessToken()
        {
            var client = new RestClient($"{_options.Value.BaseUrl}/v1/oauth2/token");
            client.Authenticator = new HttpBasicAuthenticator(_options.Value.ClientId, _options.Value.SecretKey);

            var request = new RestRequest()
            {
                Method = Method.Post
            };

            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            request.AddParameter("grant_type", "client_credentials");

            // request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful) return "";

            if (string.IsNullOrEmpty(response.Content)) return "";

            var json = response.Content.Replace("access_token", "AccessToken");

            var dto = JsonConvert.DeserializeObject<PayPalAccessTokenModel>(json);

            return dto != null ? dto.AccessToken : "";
        }

        private async Task<bool> ExecutePayment(string baseUrl, string accessToken, string payId, string payerId)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var client = new RestClient($"{baseUrl}/v1/payments/payment/{payId}/execute");

            var request = new RestRequest
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json
            };

            request.AddHeader("Authorization", $"Bearer {accessToken}");

            request.AddBody(new { payer_id = payerId });

            var response = await client.ExecuteAsync(request);

            return response.IsSuccessful;
        }
    }
}
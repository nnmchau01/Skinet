using Application.Common.Options;
using Application.Interfaces;
using Application.Models.Email;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<NotifyGoogleGmailApiOption> _notifyGmailOption;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmailService(IOptions<NotifyGoogleGmailApiOption> notifyGmailOption, IWebHostEnvironment webHostEnvironment)
    {
        _notifyGmailOption = notifyGmailOption;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task SendNewBookingAsync(string emailAdmin, string emailUser, string fullName, string dateTime,
        string serviceName, string price, string phoneNumber)
    {
        if (!string.IsNullOrEmpty(emailAdmin))
        {
            await SendWithGmailAsync(new EmailMessageModel
            {
                SendTos = new List<string>()
                    {
                        emailAdmin
                    },
                Subject = "Có một lịch hẹn mới",
            }
                , EmailFilePath.EmailHeader.ReadDescription()
                , EmailFilePath.SendMailBooking.ReadDescription()
                , "Admin"
                , "Có một lịch hẹn từ " + fullName
                , fullName
                , phoneNumber
                , dateTime
                , serviceName
                , price);
        }

        if (!string.IsNullOrEmpty(emailUser))
        {
            await SendWithGmailAsync(new EmailMessageModel
            {
                SendTos = new List<string>()
                    {
                        emailUser
                    },
                Subject = "Cám ơn bạn đã đặt lịch tại Website",
            }
                , EmailFilePath.EmailHeader.ReadDescription()
                , EmailFilePath.SendMailBooking.ReadDescription()
                , fullName
                , "Cám ơn bạn đã đặt lịch tại Website"
                , fullName
                , phoneNumber
                , dateTime
                , serviceName
                , price);
        }

    }


    public async Task SendNewOrderAsync(string emailAdmin, string emailUser, string code, string fullName, string address,
        string phoneNumber)
    {
        if (!string.IsNullOrEmpty(emailAdmin))
        {
            await SendWithGmailAsync(new EmailMessageModel
            {
                SendTos = new List<string>()
                    {
                        emailAdmin
                    },
                Subject = "Có một đơn hàng mới",
            }
                , EmailFilePath.EmailHeader.ReadDescription()
                , EmailFilePath.SendMailOrder.ReadDescription()
                , "Admin"
                , "Có một đơn hàng từ " + fullName
                , fullName
                , phoneNumber
                , address,
                code
                );
        }

        if (!string.IsNullOrEmpty(emailUser))
        {
            await SendWithGmailAsync(new EmailMessageModel
            {
                SendTos = new List<string>()
                    {
                        emailUser
                    },
                Subject = "Cám ơn bạn đã đặt hàng tại Website",
            }
                , EmailFilePath.EmailHeader.ReadDescription()
                , EmailFilePath.SendMailOrder.ReadDescription()
                , fullName
                , "Cám ơn bạn đã đặt hàng tại Website"
                , fullName
                , phoneNumber
                , address,
                code
                );
        }

    }

    private async Task SendWithGmailAsync(EmailMessageModel model, string pathHeaderEmail, string pathBodyEmail,
        params object[] parameters)
    {
        if (model.SendTos?.Count > 0)
        {
            string contentRootPath = _webHostEnvironment.WebRootPath;
            string header = Path.Combine(contentRootPath, pathHeaderEmail);
            string body = Path.Combine(contentRootPath, pathBodyEmail);

            string emailContent = await EmailHelper
                .EmailTemplateBuilder(header, body, parameters);

            #region Google Gmail API

            var accessToken = await GetGmailAccessTokenAsync(model.SendByEmail ?? string.Empty);

            foreach (var item in model.SendTos)
            {
                var client = new RestClient(_notifyGmailOption.Value.GmailAPIUrl + "/users/me/messages/send");
                var request = new RestRequest() { Method = Method.Post };
                var message = GmailMessageHelper.BuildMessage(model.Subject,
                    accessToken.SenderName,
                    item.Split("@")[0],
                    accessToken.From, item, emailContent);

                request.AddHeader("Authorization", "Bearer " + accessToken.AccessToken);
                request.AddJsonBody(new { raw = message });

                var response = await client.ExecuteAsync(request);
            }

            #endregion
        }
    }

    private async Task<GmailAPISenderInfoModel> GetGmailAccessTokenAsync(string sendByEmail = "")
    {
        var client = new RestClient(_notifyGmailOption.Value.Oauth2Url + "/token");
        var request = new RestRequest() { Method = Method.Post };

        request.AddParameter("client_id", _notifyGmailOption.Value.ClientID);
        request.AddParameter("client_secret", _notifyGmailOption.Value.ClientSecret);
        request.AddParameter("refresh_token", _notifyGmailOption.Value.RefreshToken);
        request.AddParameter("grant_type", _notifyGmailOption.Value.GrandType);

        var response = await client.ExecuteAsync(request);

        var jObject = JObject.Parse(response.Content ?? string.Empty);

        return new GmailAPISenderInfoModel()
        {
            AccessToken = jObject.ContainsKey("access_token") ? (string)jObject["access_token"]! : "",
            From = _notifyGmailOption.Value.From,
            SenderName = _notifyGmailOption.Value.SenderName
        };
    }
}
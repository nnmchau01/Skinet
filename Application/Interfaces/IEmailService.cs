namespace Application.Interfaces;

public interface IEmailService
{
    Task SendNewBookingAsync(string emailAdmin, string emailUser, string fullName, string dateTime, string serviceName, string price, string phoneNumber);

    Task SendNewOrderAsync(string emailAdmin, string emailUser,string code, string fullName, string address, string phoneNumber);
}
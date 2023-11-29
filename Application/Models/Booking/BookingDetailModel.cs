using Application.Common.Models;
using Domain.Entities;

namespace Application.Models.Booking;

public class BookingDetailModel : AuditModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Note { get; set; }
    public string ServiceName { get; set; }
    public double Price { get; set; }
    public string Date { get; set; }
    public string ExpertDate { get; set; }
    public string Time { get; set; }
}
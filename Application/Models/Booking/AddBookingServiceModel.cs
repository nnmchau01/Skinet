using System.ComponentModel.DataAnnotations;

namespace Application.Models.Booking;

public class AddBookingServiceModel
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string? Note { get; set; }
    [Required]
    public string ServiceId { get; set; }
    [Required]
    public string Date { get; set; }
    public string? ExpertDate { get; set; }
    [Required]
    public string Time { get; set; }
}
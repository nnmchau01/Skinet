using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class BookingService : Audit
{
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
    
    public string? Note { get; set; }
    
    [Required]
    public Guid ServiceId { get; set; }
    
    [Required]
    public string Date { get; set; }
    
    public string? ExpertDate { get; set; }
    
    [Required]
    public string Time { get; set; }
}
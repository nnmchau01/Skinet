using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Service : Audit
{
    [Required]
    public string ServiceName { get; set; }
    
    [Required]
    public string ServiceCode { get; set; }
    
    [Required]
    public double Price { get; set; }
}
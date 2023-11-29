using System.ComponentModel.DataAnnotations;

namespace Application.Models.Service;

public class UpdateServiceModel
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string ServiceName { get; set; }
    [Required]
    public double Price { get; set; }
}
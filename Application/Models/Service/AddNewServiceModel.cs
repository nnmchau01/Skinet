using System.ComponentModel.DataAnnotations;

namespace Application.Models.Service;

public class AddNewServiceModel
{
    [Required] public string ServiceName { get; set; }
    [Required] public double Price { get; set; }
}
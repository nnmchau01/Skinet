using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Brands;

public class AddNewBrandModel : AuditModel
{
    [Required] public string Name { get; set; }
    [Required] public string Code { get; set; }
}
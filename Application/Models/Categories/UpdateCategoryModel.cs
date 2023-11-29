using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Categories;

public class UpdateCategoryModel : AuditModel
{
    [Required] public string Name { get; set; }
    [Required] public string Code { get; set; }
}
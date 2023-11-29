using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : Audit
{
    [Required] [MaxLength(500)] public string Name { get; set; }
    [Required] [MaxLength(500)] public string Code { get; set; }
}
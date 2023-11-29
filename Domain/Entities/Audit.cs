using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Audit
{
    [Key] public Guid Id { get; set; }
    [Required] public bool IsDeleted { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [MaxLength(255)] public string? UpdatedBy { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] [MaxLength(255)] public string CreatedBy { get; set; }
}
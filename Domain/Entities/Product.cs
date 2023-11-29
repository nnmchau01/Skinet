using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product : Audit
{
    [Required] [MaxLength(500)] public string Name { get; set; }
    [MaxLength(4000)] public string? Description { get; set; }
    [Required] public decimal Price { get; set; }
    public decimal Discount { get; set; }
    [Required] public string Currency { get; set; }
    public string? DefaultImage { get; set; }
    public string OriginLinkDetail { get; set; }
    [Required] public string Url { get; set; }
    [Required] public int Stock { get; set; }
    public Guid StatusId { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
}
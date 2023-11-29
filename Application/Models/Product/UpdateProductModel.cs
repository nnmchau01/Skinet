using System.ComponentModel.DataAnnotations;
using Application.Models.Images;
using Application.Models.Property;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Product;

public class UpdateProductModel
{
    [Required] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    [Required] public decimal Price { get; set; }
    public decimal Discount { get; set; }
    [Required] public string Currency { get; set; }
    public List<IFormFile> Images { get; set; }
    public List<PropertyDetailModel>? Properties { get; set; }
    [Required] public Guid BrandId { get; set; }
    [Required] public Guid CategoryId { get; set; }
    public List<ImageDetailModel>? ListImage { get; set; }
}
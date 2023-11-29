using Application.Common.Models;
using Application.Models.Images;
using Application.Models.Property;

namespace Application.Models.Product;

public class ProductDetailModel : AuditModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Discount { get; set; }
    public string Currency { get; set; }
    public string OriginLinkDetail { get; set; }
    public string Url { get; set; }
    public int Stock { get; set; }
    public string Brand { get; set; }
    public string BrandCode { get; set; }
    public string CategoryCode { get; set; }
    public string DefaultImage { get; set; }
    public string Category { get; set; }
    public List<PropertyDetailModel> Properties { get; set; }
    public List<ImageDetailModel> Images { get; set; }
}
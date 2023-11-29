using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Images;

namespace Application.Models.Product;

public class ProductFilterModel
{
    public List<CategoryDetailModel> Categories { get; set; }
    public List<BrandDetailModel> Brands { get; set; }
    public ProductDetailModel Product { get; set; }
}
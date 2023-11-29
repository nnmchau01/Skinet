using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Product;
using Domain.Helpers;

namespace Application.Models.Search;

public class SearchPageModel
{
    public PagingResult<ProductDetailModel> PagingResult { get; set; }
    public List<CategoryDetailModel> Categories { get; set; }
    public List<BrandDetailModel> Brands { get; set; }
}
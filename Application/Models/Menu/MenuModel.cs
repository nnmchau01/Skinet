using Application.Models.Brands;
using Application.Models.Categories;

namespace Application.Models.Menu;

public class MenuModel
{
    public List<BrandDetailModel> Brands { get; set; }
    public List<CategoryDetailModel> Categories { get; set; }
    public bool IsShowButtonAdmin { get; set; }
}
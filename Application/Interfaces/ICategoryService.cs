using Application.Common.Models;
using Application.Models.Categories;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDetailModel>> GetListCategory();
    Task<CategoryDetailModel> GetDetail(string id);
    Task<bool> Delete(DeleteModel model);
    Task<bool> AddNew(AddNewCategoryModel model);
    Task<bool> Update(UpdateCategoryModel model);
}
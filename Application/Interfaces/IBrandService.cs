using Application.Common.Models;
using Application.Models.Brands;

namespace Application.Interfaces;

public interface IBrandService
{
    Task<List<BrandDetailModel>> GetListBrand();
    Task<BrandDetailModel> GetDetail(string id);
    Task<bool> Delete(DeleteModel model);
    Task<bool> AddNew(AddNewBrandModel model);
    Task<bool> Update(UpdateBrandModel model);
}
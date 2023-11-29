using Application.Common.Models;
using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Promotion;

namespace Application.Interfaces;

public interface IPromotionService
{
    Task<List<PromotionDetailModel>> GetListPromotion();
    Task<PromotionDetailModel> GetDetail(string id);
    Task<PromotionDetailModel> GetDetailByCode(string code);
    Task<bool> Delete(DeleteModel model);
    Task<bool> AddNew(AddNewPromotionModel model);
    Task<bool> Update(UpdatePromotionModel model);
}
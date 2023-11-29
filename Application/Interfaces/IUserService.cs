using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.User;

namespace Application.Interfaces;

public interface IUserService
{
    Task<List<UserDetailModel>> GetUsers();
    Task<bool> Delete(string id);
    Task<UserDetailModel> GetDetail(string userId);
}
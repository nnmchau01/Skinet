using Application.Models.Menu;
using Application.Models.Status;

namespace Application.Interfaces;

public interface ICommonService
{
    Task<MenuModel> GetMenu(bool isAdmin);
    Task<List<StatusDetailModel>> GetDropdownStatus(string type);
}
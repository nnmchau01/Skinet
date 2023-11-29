using Application.Models.Rating;
using Application.Models.Service;

namespace Application.Interfaces;

public interface IServiceService
{
    Task<List<ServiceDetailModel>> GetList();
    Task<bool> Add(AddNewServiceModel model);
    Task<bool> Update(UpdateServiceModel model);
    Task<bool> Delete(string serviceId);
    Task<ServiceDetailModel> GetDetail(string serviceId);
}
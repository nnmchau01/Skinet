using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Order;
using Application.Models.User;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderDetailModel>> GetListOrder();
    Task<OrderDetailModel> Detail(string id);
    Task<OrderDetailModel> DetailByCode(string code);
    Task<bool> Update(UpdateStatusOrderModel model);
    Task<string> AddNew(AddNewOrderModel model);
    Task<List<ChartReportSummaryModel>> GenerateOrderReport(int year, int month);
}
using Application.Common.Models;
using Application.Models.Status;

namespace Application.Models.Order;

public class OrderFilterModel : AuditModel
{
    public OrderDetailModel OrderDetail { get; set; }
    public List<StatusDetailModel> ListOrderStatus { get; set; }
    public List<StatusDetailModel> ListPaymentStatus { get; set; }
}
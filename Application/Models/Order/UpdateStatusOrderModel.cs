using Application.Common.Models;
using Application.Models.Cart;

namespace Application.Models.Order;

public class UpdateStatusOrderModel : AuditModel
{
    public Guid OrderId { get; set; }
    public Guid StatusOrderId { get; set; }
    public Guid StatusPaymentId { get; set; }
}
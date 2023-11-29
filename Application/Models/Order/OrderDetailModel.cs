using Application.Common.Models;

namespace Application.Models.Order;

public class OrderDetailModel : AuditModel
{
    public OrderDetailModel()
    {
        Items = new List<OrderItemsDetail>();
    }

    public string Code { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Note { get; set; }
    public string PaymentMethod { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public Guid OrderStatusId { get; set; }
    public Guid PaymentStatusId { get; set; }
    public int DiscountPercent { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatusCode { get; set; }
    public string PaymentStatusCode { get; set; }
    public List<OrderItemsDetail> Items { get; set; }
}
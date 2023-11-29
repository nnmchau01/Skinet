using Application.Common.Models;

namespace Application.Models.Order;

public class OrderItemsDetail : AuditModel
{
    public int Quantity { get; set; }
    public decimal Price { set; get; }
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public string PromotionCode { get; set; }
    public Guid ProductId { get; set; }
    public decimal PriceEachItem { get; set; }
    public decimal TotalPriceEachRowItem { get; set; }
    public decimal TotalPriceAll { get; set; }
    public string DefaultImage { get; set; }
    public string Currency { get; set; }
}
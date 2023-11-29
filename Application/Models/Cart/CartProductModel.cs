namespace Application.Models.Cart;

public class CartProductModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceEachItem { get; set; }
    public decimal TotalPriceEachRowItem { get; set; }
    public decimal TotalPriceAll { get; set; }
    public string DefaultImage { get; set; }
    public string ProductName { get; set; }
    public string Currency { get; set; }
    public string PromotionCode { get; set; }
}
using Application.Models.Cart;

namespace Application.Models.Checkout;

public class CheckoutModel
{
    public string PaymentMethod { get; set; }
    public List<CartProductModel> Carts { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Note { get; set; }
    public Guid PaymentId { get; set; }
    public Guid StatusId { get; set; }
}
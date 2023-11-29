using Application.Models.Cart;

namespace Application.Interfaces;

public interface ICartService
{
    Task<List<CartProductModel>> GetListCart(List<CartProductModel> list);
}
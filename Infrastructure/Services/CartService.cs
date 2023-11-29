using Application.Interfaces;
using Application.Models.Brands;
using Application.Models.Cart;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CartService : ICartService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPromotionService _promotionService;

    public CartService(IApplicationDbContext dbContext, IPromotionService promotionService)
    {
        _dbContext = dbContext;
        _promotionService = promotionService;
    }

    public async Task<List<CartProductModel>> GetListCart(List<CartProductModel> list)
    {
        var products = await _dbContext.Products
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        var listJoin = from product in products
            join cart in list on product.Id equals cart.ProductId
            where cart.Quantity > 0
            select new
            {
                ProductId = product.Id, Quantity = cart.Quantity, PriceEachItem = product.Price,
                TotalPriceEachRowItem = product.Discount > 0
                    ? (product.Price.CalDiscountPrice(product.Discount) * cart.Quantity)
                    : product.Price * cart.Quantity,
                TotalPriceAll = 0,
                DefaultImage = product.DefaultImage,
                ProductName = product.Name,
                Currency = product.Currency,
                PromotionCode = cart.PromotionCode
            };

        var dataReturn = listJoin.Adapt<List<CartProductModel>>();

        if (!(dataReturn?.Count > 0)) return new List<CartProductModel>();

        var total = dataReturn.Sum(price => (double)price.TotalPriceEachRowItem);

        foreach (var price in dataReturn)
        {
            price.TotalPriceAll = (decimal)total;
        }

        var promotion = await _promotionService.GetDetailByCode(dataReturn.First().PromotionCode);

        if (string.IsNullOrEmpty(promotion?.Code)) return dataReturn;

        foreach (var price in dataReturn)
        {
            price.TotalPriceAll = price.TotalPriceAll.CalDiscountPrice(promotion.DiscountPercent);
        }

        return dataReturn;
    }
}
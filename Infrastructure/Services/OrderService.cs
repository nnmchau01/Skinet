using Application.Interfaces;
using Application.Models.Order;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _dbContext;

    public OrderService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderDetailModel>> GetListOrder()
    {
        var orders = await _dbContext.Orders.Where(s => !s.IsDeleted).ToListAsync();
        var statuses = await _dbContext.Status.Where(s => !s.IsDeleted).ToListAsync();
        var payments = await _dbContext.Payments.Where(s => !s.IsDeleted).ToListAsync();

        var listJoin = from order in orders
            join statusOrder in statuses on order.StatusId equals statusOrder.Id
            join payment in payments on order.PaymentId equals payment.Id
            join statusPayment in statuses on payment.StatusId equals statusPayment.Id
            select new
            {
                Id = order.Id, CustomerName = order.CustomerName, Address = order.Address,
                PhoneNumber = order.PhoneNumber, Note = order.Note,
                PaymentMethod = payment.PaymentMethod, OrderStatus = statusOrder.Display,
                PaymentStatus = statusPayment.Display, DiscountPercent = 0,
                TotalAmount = order.TotalAmount, CreatedAt = order.CreatedAt, Code = order.Code, 
                OrderStatusCode = statusOrder.Code,
                PaymentStatusCode = statusPayment.Code
            };

        var data = listJoin.Adapt<List<OrderDetailModel>>().OrderByDescending(s => s.CreatedAt).ToList();

        return data;
    }

    public async Task<OrderDetailModel> Detail(string id)
    {
        var orders = await _dbContext.Orders.Where(s => !s.IsDeleted && s.Id == Guid.Parse(id)).ToListAsync();

        if (orders.Count == 0)
        {
            return new OrderDetailModel();
        }

        var statuses = await _dbContext.Status.Where(s => !s.IsDeleted).ToListAsync();
        var payments = await _dbContext.Payments.Where(s => !s.IsDeleted && s.Id == orders.First().PaymentId)
            .ToListAsync();

        var listJoin = from order in orders
            join statusOrder in statuses on order.StatusId equals statusOrder.Id
            join payment in payments on order.PaymentId equals payment.Id
            join statusPayment in statuses on payment.StatusId equals statusPayment.Id
            select new
            {
                Id = order.Id, CustomerName = order.CustomerName, Address = order.Address,
                PhoneNumber = order.PhoneNumber, Note = order.Note,
                PaymentMethod = payment.PaymentMethod, OrderStatus = statusOrder.Display,
                PaymentStatus = statusPayment.Display, DiscountPercent = 0,
                TotalAmount = order.TotalAmount, CreatedAt = order.CreatedAt, Code = order.Code,
                OrderStatusId = statusOrder.Id, PaymentStatusId = statusPayment.Id,
                OrderStatusCode = statusOrder.Code,
                PaymentStatusCode = statusPayment.Code
            };

        var data = listJoin.Adapt<List<OrderDetailModel>>();
        var detailFinal = data.First();

        var orderItems = await _dbContext.OrderItems.Where(s => !s.IsDeleted && s.OrderId == detailFinal.Id)
            .ToListAsync();
        var productIds = orderItems.Select(s => s.ProductId).ToList();
        var products = await _dbContext.Products.Where(s => !s.IsDeleted && productIds.Contains(s.Id)).ToListAsync();
        var promo = await _dbContext.Promotions.FirstOrDefaultAsync(s =>
            !s.IsDeleted && s.Id == orderItems.First().PromotionId);

        var listItemJoin = from item in orderItems
            join product in products on item.ProductId equals product.Id
            select new
            {
                ProductId = product.Id, Quantity = item.Quantity, PriceEachItem = product.Price,
                TotalPriceEachRowItem = product?.Discount > 0
                    ? (product.Price.CalDiscountPrice(product.Discount) * item.Quantity)
                    : product.Price * item.Quantity,
                TotalPriceAll = 0,
                DefaultImage = product.DefaultImage,
                ProductName = product.Name,
                Currency = product.Currency,
                PromotionCode = promo != null ? promo.Code : "",
                DiscountPercent = promo != null ? promo.DiscountPercent : 0,
            };

        detailFinal.Items = listItemJoin.Adapt<List<OrderItemsDetail>>();
        detailFinal.DiscountPercent = promo != null ? promo.DiscountPercent : 0;

        return detailFinal;
    }

    public async Task<OrderDetailModel> DetailByCode(string code)
    {
        var orders = await _dbContext.Orders.Where(s => !s.IsDeleted && s.Code == code).ToListAsync();

        if (orders.Count == 0)
        {
            return new OrderDetailModel();
        }

        var statuses = await _dbContext.Status.Where(s => !s.IsDeleted).ToListAsync();
        var payments = await _dbContext.Payments.Where(s => !s.IsDeleted && s.Id == orders.First().PaymentId)
            .ToListAsync();

        var listJoin = from order in orders
            join statusOrder in statuses on order.StatusId equals statusOrder.Id
            join payment in payments on order.PaymentId equals payment.Id
            join statusPayment in statuses on payment.StatusId equals statusPayment.Id
            select new
            {
                Id = order.Id, CustomerName = order.CustomerName, Address = order.Address,
                PhoneNumber = order.PhoneNumber, Note = order.Note,
                PaymentMethod = payment.PaymentMethod, OrderStatus = statusOrder.Display,
                PaymentStatus = statusPayment.Display, DiscountPercent = 0,
                TotalAmount = order.TotalAmount, CreatedAt = order.CreatedAt, Code = order.Code,
                OrderStatusCode = statusOrder.Code,
                PaymentStatusCode = statusPayment.Code
            };

        var data = listJoin.Adapt<List<OrderDetailModel>>();

        var detailFinal = data.First();

        var orderItems = await _dbContext.OrderItems.Where(s => !s.IsDeleted && s.OrderId == detailFinal.Id)
            .ToListAsync();
        var productIds = orderItems.Select(s => s.ProductId).ToList();
        var products = await _dbContext.Products.Where(s => !s.IsDeleted && productIds.Contains(s.Id)).ToListAsync();
        var promo = await _dbContext.Promotions.FirstOrDefaultAsync(s =>
            !s.IsDeleted && s.Id == orderItems.First().PromotionId);

        var listItemJoin = from item in orderItems
            join product in products on item.ProductId equals product.Id
            select new
            {
                ProductId = product.Id, Quantity = item.Quantity, PriceEachItem = product.Price,
                TotalPriceEachRowItem = product?.Discount > 0
                    ? (product.Price.CalDiscountPrice(product.Discount) * item.Quantity)
                    : product.Price * item.Quantity,
                TotalPriceAll = 0,
                DefaultImage = product.DefaultImage,
                ProductName = product.Name,
                Currency = product.Currency,
                PromotionCode = promo != null ? promo.Code : "",
                DiscountPercent = promo != null ? promo.DiscountPercent : 0,
            };

        detailFinal.Items = listItemJoin.Adapt<List<OrderItemsDetail>>();
        detailFinal.DiscountPercent = promo != null ? promo.DiscountPercent : 0;

        return detailFinal;
    }

    public async Task<bool> Update(UpdateStatusOrderModel model)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.OrderId);
        if (order == null) return false;

        var payment = await _dbContext.Payments.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == order.PaymentId);
        if (payment == null) return false;

        order.StatusId = model.StatusOrderId;
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        payment.StatusId = model.StatusPaymentId;
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<string> AddNew(AddNewOrderModel model)
    {
        var status = await _dbContext.Status.Where(s => !s.IsDeleted).ToListAsync();
        Guid paymentStatus;
        var orderId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var orderStatusAwaiting = status.First(s =>
                s.Code == OrderStatus.AwaitingShipCode.ReadDescription() &&
                s.Type == StatusType.Order.ReadDescription())
            .Id;

        paymentStatus = model.PaymentMethod.ToLower() switch
        {
            "cash" => status.First(s =>
                    s.Code == PaymentStatus.WaitingPayCode.ReadDescription() &&
                    s.Type == StatusType.Payment.ReadDescription())
                .Id,
            "vnpay" => status.First(s =>
                    s.Code == PaymentStatus.PaidCode.ReadDescription() &&
                    s.Type == StatusType.Payment.ReadDescription())
                .Id,
            "paypal" => status.First(s =>
                    s.Code == PaymentStatus.PaidCode.ReadDescription() &&
                    s.Type == StatusType.Payment.ReadDescription())
                .Id,
            "momo" => status.First(s =>
                    s.Code == PaymentStatus.PaidCode.ReadDescription() &&
                    s.Type == StatusType.Payment.ReadDescription())
                .Id,
            _ => throw new Exception("Unsupported")
        };


        var promo = await _dbContext.Promotions.FirstOrDefaultAsync(s =>
            !s.IsDeleted && s.Code == model.Carts.First().PromotionCode);
        var promotionId = promo?.Id;
        var discount = promo?.DiscountPercent;
        var fee = discount ?? 0;
        var order = model.Adapt<Order>();

        order.Id = orderId;
        order.Code = Guid.NewGuid().ToString().Split("-")[0].ToUpper();
        order.StatusId = orderStatusAwaiting;
        order.PaymentId = paymentId;
        order.TotalAmount = model.Carts.First().TotalPriceAll;

        var payment = new Payment()
        {
            Amount = model.Carts.First().TotalPriceAll,
            Fee = fee,
            PaymentMethod = model.PaymentMethod,
            Id = paymentId,
            StatusId = paymentStatus,
            TransactionDate = DateTime.Now,
            PaymentCode = paymentId.ToString().Split("-")[0].ToUpper(),
            TransactionId = DateTime.Now.Ticks.ToString()
        };

        if (model.PaymentMethod.ToLower() == "cash")
        {
            payment.PaymentMethod = AppVersion.IsEnglishVersion
                ? PaymentMethod.CashDisplayEN.ReadDescription()
                : PaymentMethod.CashDisplayVN.ReadDescription();
        }

        var orderItems = model.Carts.Select(item => new OrderItem()
            {
                Id = Guid.NewGuid(),
                Price = item.PriceEachItem,
                Quantity = item.Quantity,
                OrderId = orderId,
                ProductId = item.ProductId,
                PromotionId = promotionId ?? Guid.NewGuid()
            })
            .ToList();

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        await _dbContext.OrderItems.AddRangeAsync(orderItems);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        foreach (var product in orderItems)
        {
            var p = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == product.ProductId && !s.IsDeleted);

            if (p != null)
            {
                var totalProduct = product.Quantity;
                
                p.Stock = p.Stock - product.Quantity;
                
                _dbContext.Products.Update(p);
                await _dbContext.SaveChangesAsync(new CancellationToken());
            }
            
        }
        
        return order.Code;
    }

    public async Task<List<ChartReportSummaryModel>> GenerateOrderReport(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1);
        var payments = await _dbContext.Payments.Where(s => !s.IsDeleted).ToListAsync();
        var status = await _dbContext.Status.Where(s => !s.IsDeleted).ToListAsync();
        
        var orders = await _dbContext.Orders
            .Where(s => !s.IsDeleted && s.CreatedAt >= startDate && s.CreatedAt < endDate)
            .ToListAsync();

        var ordersPaid = from order in orders
            join payment in payments on order.PaymentId equals payment.Id
            join stt in status on payment.StatusId equals stt.Id
            where stt.Code == PaymentStatus.PaidCode.ReadDescription()
            select new
            {
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            };

        var orderData = ordersPaid.Adapt<List<Order>>();
        
        var reportData = new List<ChartReportSummaryModel>();

        for (var day = 1; day <= DateTime.DaysInMonth(year, month); day++)
        {
            var ordersInDay = orderData.Where(s => s.CreatedAt.Day == day).ToList();
            var totalAmount = (double)ordersInDay.Sum(s => s.TotalAmount);

            var chartModel = new ChartReportSummaryModel
            {
                ValueX = day,
                ValueY = totalAmount
            };

            reportData.Add(chartModel);
        }

        return reportData;
    }
}
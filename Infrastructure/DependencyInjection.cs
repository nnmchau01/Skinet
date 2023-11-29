using Application.Common.Options;
using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IInitDataService, InitDataService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IVNPayService, VNPayService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPayPalService, PayPalService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IMomoService, MomoService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.Configure<VnPayOption>(configuration.GetSection("PaymentConfig:VnPay"));
        services.Configure<PaypalOption>(configuration.GetSection("PaymentConfig:Paypal"));
        services.Configure<MoMoOption>(configuration.GetSection("PaymentConfig:MoMo"));
        services.Configure<NotifyGoogleGmailApiOption>(configuration.GetSection("GoogleAPI:NotifyGmail"));
        
        return services;
    }
}
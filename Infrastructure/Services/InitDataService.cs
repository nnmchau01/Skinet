using Application.Common.Models;
using Domain.Constants;
using Application.Interfaces;
using Application.Models.Brands;
using Application.Models.Categories;
using Application.Models.Promotion;
using Application.Models.Status;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class InitDataService : IInitDataService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public InitDataService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IApplicationDbContext dbContext,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task InitData()
    {
        await InitRoles();
        await InitUsers();
        await InitStatus();
        await InitBrands();
        await InitCategories();
        await InitPromotions();
    }

    private async Task InitRoles()
    {
        foreach (var role in SecurityRoles.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Description = $"Role for {role}"
                });
            }
        }
    }

    private async Task InitUsers()
    {
        #region data for init

        List<UserInitModel> data = new()
        {
            // full role
            new UserInitModel
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.ToList(),
                FullName = "Admin",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = AppConst.Domain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            },
            // ignore admin
            new UserInitModel
            {
                Id = Guid.NewGuid(),
                UserName = "user",
                Email = "user@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.Where(s => !s.Equals(SecurityRoles.Admin)
                                                       && !s.Equals(SecurityRoles.Manager)).ToList(),
                FullName = "User",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = AppConst.Domain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            },
            new UserInitModel
            {
                Id = Guid.NewGuid(),
                UserName = "manager",
                Email = "manager@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.Where(s => !s.Equals(SecurityRoles.Admin)).ToList(),
                FullName = "Manager",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = AppConst.Domain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            }
        };

        #endregion

        foreach (var item in data)
        {
            var user = await _userManager.FindByNameAsync(item.UserName);

            if (user == null)
            {
                await _userManager.CreateAsync(item.Adapt<ApplicationUser>(), _configuration["DefaultPassword"]);
            }

            var userExist = await _userManager.FindByNameAsync(item.UserName);

            if (userExist == null)
            {
                continue;
            }

            await _userManager.RemoveFromRolesAsync(userExist, SecurityRoles.Roles);
            await _userManager.AddToRolesAsync(userExist, SecurityRoles.Roles);
        }
    }

    private async Task InitStatus()
    {
        #region data for status

        var data = new List<AddNewStatusModel>()
        {
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Order.ReadDescription(),
                Code = OrderStatus.ReceivedCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? OrderStatus.ReceivedDisplayEN.ReadDescription()
                    : OrderStatus.ReceivedDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Order.ReadDescription(),
                Code = OrderStatus.CancelCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? OrderStatus.CancelDisplayEN.ReadDescription()
                    : OrderStatus.CancelDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Order.ReadDescription(),
                Code = OrderStatus.RejectCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? OrderStatus.RejectDisplayEN.ReadDescription()
                    : OrderStatus.RejectDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Order.ReadDescription(),
                Code = OrderStatus.AwaitingShipCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? OrderStatus.AwaitingShipDisplayEN.ReadDescription()
                    : OrderStatus.AwaitingShipDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Order.ReadDescription(),
                Code = OrderStatus.ShippingCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? OrderStatus.ShippingDisplayEN.ReadDescription()
                    : OrderStatus.ShippingDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Product.ReadDescription(),
                Code = ProductStatus.AvailableCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? ProductStatus.AvailableDisplayEN.ReadDescription()
                    : ProductStatus.AvailableDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Product.ReadDescription(),
                Code = ProductStatus.NotAvailableCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? ProductStatus.NotAvailableDisplayEN.ReadDescription()
                    : ProductStatus.NotAvailableDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Payment.ReadDescription(),
                Code = PaymentStatus.PaidCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? PaymentStatus.PaidDisplayEN.ReadDescription()
                    : PaymentStatus.PaidDisplayVN.ReadDescription(),
            },
            new AddNewStatusModel()
            {
                Id = Guid.NewGuid(),
                Type = StatusType.Payment.ReadDescription(),
                Code = PaymentStatus.WaitingPayCode.ReadDescription(),
                Display = AppVersion.IsEnglishVersion
                    ? PaymentStatus.WaitingPayDisplayEN.ReadDescription()
                    : PaymentStatus.WaitingPayDisplayVN.ReadDescription(),
            }
        };

        #endregion

        var dataAdapt = data.Adapt<List<Status>>();
        var dataFinal = new List<Status>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Status.FirstOrDefaultAsync(s => s.Display == item.Display);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Status.AddRangeAsync(dataFinal);
            await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }

    private async Task InitBrands()
    {
        #region data for brands

        var data = new List<AddNewBrandModel>()
        {
            new AddNewBrandModel()
            {
                Code = "binh-gold",
                Name = "Binh Gold"
            },
            new AddNewBrandModel()
            {
                Code = "fm-style",
                Name = "FM Style"
            },
            new AddNewBrandModel()
            {
                Code = "zara",
                Name = "Zara"
            },
            new AddNewBrandModel()
            {
                Code = "hm",
                Name = "HM"
            }
        };

        #endregion

        var dataAdapt = data.Adapt<List<Brand>>();
        var dataFinal = new List<Brand>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Brands.FirstOrDefaultAsync(s => s.Name == item.Name);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Brands.AddRangeAsync(dataFinal);
            await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }

    private async Task InitCategories()
    {
        #region data for categories

        var data = new List<AddNewCategoryModel>()
        {
            new AddNewCategoryModel()
            {
                Code = "luxury",
                Name = "Luxury"
            },
            new AddNewCategoryModel()
            {
                Code = "sport",
                Name = "Thể thao"
            },
            new AddNewCategoryModel()
            {
                Code = "male",
                Name = "Quần áo nam"
            },
            new AddNewCategoryModel()
            {
                Code = "female",
                Name = "Quần áo nữ"
            }
        };

        #endregion

        var dataAdapt = data.Adapt<List<Category>>();
        var dataFinal = new List<Category>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Categories.FirstOrDefaultAsync(s => s.Name == item.Name);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Categories.AddRangeAsync(dataFinal);
            await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }

    private async Task InitPromotions()
    {
        #region data for brands

        var data = new List<AddNewPromotionModel>()
        {
            new AddNewPromotionModel()
            {
                Code = "CODEMEGA1",
                DiscountPercent = 10
            },
            new AddNewPromotionModel()
            {
                Code = "CODEMEGA2",
                DiscountPercent = 12
            },
            new AddNewPromotionModel()
            {
                Code = "CODEMEGA3",
                DiscountPercent = 15
            },
            new AddNewPromotionModel()
            {
                Code = "CODEMEGA4",
                DiscountPercent = 20
            },
        };

        #endregion

        var dataAdapt = data.Adapt<List<Promotion>>();
        var dataFinal = new List<Promotion>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Promotions.FirstOrDefaultAsync(s => s.Code == item.Code);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Promotions.AddRangeAsync(dataFinal);
            await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}
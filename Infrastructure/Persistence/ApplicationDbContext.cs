using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    private readonly IIdentityService _identityService;

    public ApplicationDbContext(DbContextOptions options, IIdentityService identityService)
        : base(options)
    {
        _identityService = identityService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Tables

        builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        builder.Entity<ApplicationRole>().ToTable("ApplicationRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("ApplicationUserClaims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("ApplicationUserRoles")
            .HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("ApplicationUserLogins").HasKey(x => x.UserId);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("ApplicationRoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("ApplicationUserTokens").HasKey(x => x.UserId);

        builder.Entity<Brand>().ToTable("Brands");
        builder.Entity<Category>().ToTable("Categories");
        builder.Entity<Image>().ToTable("Images");
        builder.Entity<Order>().ToTable("Orders");
        builder.Entity<OrderItem>().ToTable("OrderItems");
        builder.Entity<Payment>().ToTable("Payments");
        builder.Entity<Product>().ToTable("Products");
        builder.Entity<Promotion>().ToTable("Promotions");
        builder.Entity<Property>().ToTable("Properties");
        builder.Entity<Status>().ToTable("Status");
        builder.Entity<Rating>().ToTable("Ratings");
        builder.Entity<BookingService>().ToTable("BookingServices");
        builder.Entity<Service>().ToTable("Services");
        
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #endregion

        #region Set Infomation Coulumn

        builder.Entity<ApplicationUser>()
            .Property(x => x.UserName)
            .HasMaxLength(300);
        builder.Entity<ApplicationUser>()
            .Property(x => x.NormalizedUserName)
            .HasMaxLength(300);
        builder.Entity<ApplicationUser>()
            .Property(x => x.Email)
            .HasMaxLength(300);
        builder.Entity<ApplicationUser>()
            .Property(x => x.NormalizedEmail)
            .HasMaxLength(300);

        builder.Entity<ApplicationUser>()
            .HasIndex(x => new { x.UserName, x.NormalizedUserName, x.Email, x.NormalizedEmail })
            .IsUnique();

        builder.Entity<IdentityUserRole<Guid>>()
            .HasIndex(x => new { x.UserId, x.RoleId })
            .IsUnique(false);

        #endregion
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<BookingService> BookingServices { get; set; }

    public override EntityEntry Entry(object entity) => base.Entry(entity);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userChange = string.IsNullOrEmpty(_identityService.GetCurrentUserLogin().Id)
            ? AppConst.Author
            : _identityService.GetCurrentUserLogin().Id;
        var dateTimeChange = DateTime.Now;

        foreach (var entry in ChangeTracker.Entries<Audit>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userChange;
                    entry.Entity.CreatedAt = dateTimeChange;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userChange;
                    entry.Entity.UpdatedAt = dateTimeChange;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.Entity.UpdatedBy = userChange;
                    entry.Entity.UpdatedAt = dateTimeChange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
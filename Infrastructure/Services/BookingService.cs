using Application.Interfaces;
using Application.Models.Booking;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _dbContext;

    public BookingService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookingDetailModel>> GetList()
    {
        var bookings = await _dbContext.BookingServices
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var services = await _dbContext.Services
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var dataJoin = from b in bookings
            join s in services on b.ServiceId equals s.Id
            select new
            {
                b.Id,
                b.FullName,
                b.Email,
                b.PhoneNumber,
                b.Note,
                s.ServiceName,
                b.Date,
                b.ExpertDate,
                b.Time,
                s.Price
            };

        return dataJoin.Adapt<List<BookingDetailModel>>();
    }

    public async Task<bool> Add(AddBookingServiceModel model)
    {
        var newData = model.Adapt<Domain.Entities.BookingService>();

        await _dbContext.BookingServices.AddAsync(newData);

        await _dbContext.SaveChangesAsync(new CancellationToken());

        return true;
    }

    public async Task<BookingDetailModel> GetDetail(string bookingId)
    {
        var bookings = await _dbContext.BookingServices
            .Where(s => !s.IsDeleted && s.Id == Guid.Parse(bookingId))
            .ToListAsync();

        var services = await _dbContext.Services
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var dataJoin = from b in bookings
            join s in services on b.ServiceId equals s.Id
            select new
            {
                b.Id,
                b.FullName,
                b.Email,
                b.PhoneNumber,
                b.Note,
                s.ServiceName,
                b.Date,
                b.Time,
                s.Price
            };

        var list = dataJoin.Adapt<List<BookingDetailModel>>();

        var data = list.FirstOrDefault();
        
        return data == null ? new BookingDetailModel() : data.Adapt<BookingDetailModel>();
    }
}
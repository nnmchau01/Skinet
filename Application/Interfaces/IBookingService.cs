using Application.Models.Booking;

namespace Application.Interfaces;

public interface IBookingService
{
    Task<List<BookingDetailModel>> GetList();
    Task<bool> Add(AddBookingServiceModel model);
    Task<BookingDetailModel> GetDetail(string bookingId);
}
using Repository.Models;
using Service.Dtos;

namespace Service.Services.IServices
{
    public interface IBookingService
    {
        Task<List<BookingHistoryDto>> GetBookingByCusId(int id);
        Task<BookingReservation?> GetBookingById(int id);
        Task<bool> CreateBooking(BookingReservation booking);
    }
}

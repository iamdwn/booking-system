using Repository.Models;
using Service.Dtos;

namespace Repository.Repositories.Interfaces
{
    public interface IBookingReservationRepository
    {
        Task<BookingReservation?> GetBookingById(int id);
        Task<List<BookingHistoryDto>> GetBookingByCusId(int id);
        Task<bool> CreateBooking(BookingReservation booking);
    }
}

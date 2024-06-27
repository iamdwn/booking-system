using Repository.Models;
using Service.Dtos;

namespace Repository.Repositories.IRepositories
{
    public interface IBookingReservationRepository
    {
        Task<BookingReservation?> GetBookingById(int id);
        Task<List<BookingHistoryDto>> GetBookingByCusId(int id);
        Task<bool> CreateBooking(BookingReservation booking);
    }
}

using Repository.Models;

namespace Repository.Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        Task<bool> CreateBookingDetail(BookingDetail bookingDetail);
        Task<List<BookingDetail>> GetBookingDetails();
    }
}

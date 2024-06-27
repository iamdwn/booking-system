using Repository.Models;

namespace Repository.Repositories.IRepositories
{
    public interface IBookingDetailRepository
    {
        Task<bool> CreateBookingDetail(BookingDetail bookingDetail);
        Task<List<BookingDetail>> GetBookingDetails();
    }
}

using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.IRepositories;

namespace Repository.Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingDetailRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookingDetail(BookingDetail bookingDetail)
        {
            _context.BookingDetails.Add(bookingDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<BookingDetail>> GetBookingDetails()
        {
            return await _context.BookingDetails.Include(bd => bd.Room).ToListAsync();
        }
    }
}

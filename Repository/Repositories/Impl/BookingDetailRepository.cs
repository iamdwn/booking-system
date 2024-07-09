using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Impl
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
            try
            {
                _context.BookingDetails.Add(bookingDetail);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }

        public async Task<List<BookingDetail>> GetBookingDetails()
        {
            try
            {
                return await _context.BookingDetails.Include(bd => bd.Room).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }
    }
}

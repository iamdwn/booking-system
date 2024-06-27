using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.IRepositories;
using Service.Dtos;

namespace Repository.Repositories
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingReservationRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBooking(BookingReservation booking)
        {
            _context.BookingReservations.Add(booking);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<BookingHistoryDto>> GetBookingByCusId(int id)
        {
            var result = await _context.BookingReservations
                .Where(b => b.CustomerId == id)
                .Select(b => new BookingHistoryDto
                {
                    CustomerId = b.CustomerId,
                    ActualPrice = b.TotalPrice,
                    BookingDate = b.BookingDate,
                    BookingStatus = b.BookingStatus,
                    BookingReservationId = b.BookingReservationId,
                })
                .ToListAsync();
            return result;
        }

        public async Task<BookingReservation?> GetBookingById(int id)
        {
            return await _context.BookingReservations
                .Include(b => b.BookingDetails)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingReservationId == id);
        }
    }
}

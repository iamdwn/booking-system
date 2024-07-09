using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.Interfaces;
using Service.Dtos;

namespace Repository.Repositories.Impl
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
            try
            {
                _context.BookingReservations.Add(booking);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }

        public async Task<List<BookingHistoryDto>> GetBookingByCusId(int id)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }

        public async Task<BookingReservation?> GetBookingById(int id)
        {
            try
            {
                return await _context.BookingReservations
                    .Include(b => b.BookingDetails)
                    .Include(b => b.Customer)
                    .FirstOrDefaultAsync(b => b.BookingReservationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using Repository.Models;
using Repository.Repositories.Interfaces;
using Service.Dtos;
using Service.Services.Interfaces;

namespace Service.Services.Impl
{
    public class BookingService : IBookingService
    {
        private readonly IBookingReservationRepository _bookingRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;
        private readonly BookingHub _hubContext;

        public BookingService(IBookingReservationRepository bookingRepository, IRoomInformationRepository roomInformationRepository, BookingHub hubContext)
        {
            _bookingRepository = bookingRepository;
            _roomInformationRepository = roomInformationRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> CreateBooking(BookingReservation booking)
        {
                bool result = await _bookingRepository.CreateBooking(booking);
                foreach (var bookingDetail in booking.BookingDetails)
                {
                    var room = await _roomInformationRepository.GetRoomById(bookingDetail.RoomId);
                    if (room == null) return false;
                    room.RoomStatus = 0;
                    result = await _roomInformationRepository.UpdateRoom(room);
                }

                if (result)
                {
                    var data = await _roomInformationRepository.GetRooms(r => r.RoomStatus != 0);
                    await _hubContext.SendMessageWithData(data);
                }

                return result;
        }

        public async Task<List<BookingHistoryDto>> GetBookingByCusId(int id) =>  await _bookingRepository.GetBookingByCusId(id);

        public async Task<BookingReservation?> GetBookingById(int id) => await _bookingRepository.GetBookingById(id);
    }
}

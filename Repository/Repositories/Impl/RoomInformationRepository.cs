using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repository.Repositories.Impl
{
    public class RoomInformationRepository : IRoomInformationRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public RoomInformationRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<RoomInformation?> GetRoomById(int roomId)
        {
            try
            {
                return await _context.RoomInformations.FirstOrDefaultAsync(r => r.RoomId == roomId && r.RoomStatus == 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }

        public async Task<List<RoomInformation>> GetRooms(Expression<Func<RoomInformation, bool>> predicate)
        {
            try
            {
                return await _context.RoomInformations
                    .Where(predicate)
                    .Include(r => r.RoomType)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }

        public async Task<bool> UpdateRoom(RoomInformation room)
        {
            try
            {
                var existingRoom = await _context.RoomInformations.FindAsync(room.RoomId);
                if (existingRoom != null)
                {
                    existingRoom.RoomNumber = room.RoomNumber;
                    existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                    existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                    existingRoom.RoomStatus = room.RoomStatus;
                    existingRoom.RoomPricePerDay = room.RoomPricePerDay;
                    existingRoom.RoomType = room.RoomType;

                    _context.RoomInformations.Update(existingRoom);
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid booking details");
            }
        }
    }
}

using Repository.Models;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface IRoomInformationRepository
    {
        Task<List<RoomInformation>> GetRooms(Expression<Func<RoomInformation, bool>> predicate);
        Task<RoomInformation?> GetRoomById(int roomId);
        Task<bool> UpdateRoom(RoomInformation room);
    }
}

using Repository.Models;
using System.Linq.Expressions;

namespace Repository.Repositories.IRepositories
{
    public interface IRoomInformationRepository
    {
        Task<List<RoomInformation>> GetRooms(Expression<Func<RoomInformation, bool>> predicate);
        Task<RoomInformation?> GetRoomById(int roomId);
        Task<bool> UpdateRoom(RoomInformation room);
    }
}

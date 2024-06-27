using Repository.Models;
using System.Linq.Expressions;

namespace Service.Services.IServices
{
    public interface IRoomService
    {
        Task<List<RoomInformation>> GetRooms(Expression<Func<RoomInformation, bool>> predicate);
    }
}

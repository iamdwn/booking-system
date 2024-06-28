using Repository.Models;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using System.Linq.Expressions;

namespace Service.Services.Impl
{
    public class RoomService : IRoomService
    {
        private readonly IRoomInformationRepository _roomInformationRepository;

        public RoomService(IRoomInformationRepository roomInformationRepository)
        {
            _roomInformationRepository = roomInformationRepository;
        }

        public Task<List<RoomInformation>> GetRooms(Expression<Func<RoomInformation, bool>> predicate) => _roomInformationRepository.GetRooms(predicate);
    }
}

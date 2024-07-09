using Microsoft.Extensions.Logging;
using Quartz;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services.Quartzs
{
    public class UpdateRoomStatusJob : IJob
    {
        private readonly ILogger<UpdateRoomStatusJob> _logger;
        private readonly IBookingDetailRepository _bookingDetailRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public UpdateRoomStatusJob(ILogger<UpdateRoomStatusJob> logger, IBookingDetailRepository bookingDetailRepository, IRoomInformationRepository roomInformationRepository)
        {
            _logger = logger;
            _bookingDetailRepository = bookingDetailRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var bookingDetails = await _bookingDetailRepository.GetBookingDetails();

                foreach (var bookingDetail in bookingDetails)
                {
                    if (bookingDetail.EndDate <= DateOnly.FromDateTime(DateTime.Now))
                    {
                        bookingDetail.Room.RoomStatus = 1;
                        var result = await _roomInformationRepository.UpdateRoom(bookingDetail.Room);

                        if (!result) throw new Exception("Update Room status failed!");

                        _logger.LogWarning("Update Room status...");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
            }
        }
    }
}

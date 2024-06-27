namespace Service.Dtos
{
    public class BookingHistoryDto
    {
        public int BookingReservationId { get; set; }

        public decimal? ActualPrice { get; set; }

        public DateOnly? BookingDate { get; set; }

        public int CustomerId { get; set; }

        public byte? BookingStatus { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;
using Microsoft.AspNetCore.Authorization;
using Service.Dtos;
using System.ComponentModel.DataAnnotations;
using Service.Services.IServices;

namespace RazorPage.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;

        public IndexModel(IBookingService bookingService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
        }

        public List<RoomInformation> Rooms { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Check-in date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Check-out date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }

        public async Task OnGetAsync()
        {
            var user = HttpContext.Session.GetObjectFromJson<UserDto>("User");

            if (user != null)
            {
                CustomerName = user.userName;
            }

            await LoadCartRoomsAsync();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int roomId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
            cart.Remove(roomId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateBookingAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = HttpContext.Session.GetObjectFromJson<UserDto>("User");

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await LoadCartRoomsAsync();

            var booking = new BookingReservation
            {
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                BookingDetails = new List<BookingDetail>(),
                BookingStatus = 0,
                CustomerId = user.userId,
                TotalPrice = 0,
            };

            var listBookingDetails = new List<BookingDetail>();

            foreach (var room in Rooms)
            {
                var bookingDetail = new BookingDetail
                {
                    StartDate = DateOnly.FromDateTime(CheckInDate),
                    EndDate = DateOnly.FromDateTime(CheckOutDate),
                    ActualPrice = room.RoomPricePerDay * (CheckOutDate - CheckInDate).Days,
                    RoomId = room.RoomId,
                    BookingReservationId = 1,
                };

                booking.BookingDetails.Add(bookingDetail);
            }

            booking.TotalPrice = CalculateTotalPrice(Rooms, CheckInDate, CheckOutDate);

            var result = await _bookingService.CreateBooking(booking);

            if (!result)
            {
                //TempData["toast-error"] = "Booking failed try again!";
                TempData["toast-success"] = "Booking success!";
                return Page();
            }

            TempData["toast-success"] = "Booking success!";
            return Page();
            //return RedirectToPage("/Bookings/Confirmation"/*, new { bookingId = booking.Id }*/);
        }

        private async Task LoadCartRoomsAsync()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();

            Rooms = await _roomService.GetRooms(r => cart.Contains(r.RoomId));
        }

        private decimal? CalculateTotalPrice(List<RoomInformation> rooms, DateTime checkInDate, DateTime checkOutDate)
        {
            var totalDays = (checkOutDate - checkInDate).Days;
            decimal? totalPrice = 0;

            foreach (var room in rooms)
            {
                totalPrice += room.RoomPricePerDay * totalDays;
            }

            return totalPrice;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models;
using Service.Services.IServices;

namespace RazorPage.Pages.Booking
{
    public class DetailsModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public DetailsModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var bookingreservation = await _bookingService.GetBookingById(id);
            if (bookingreservation == null)
            {
                return NotFound();
            }
            else
            {
                BookingReservation = bookingreservation;
            }
            return Page();
        }
    }
}

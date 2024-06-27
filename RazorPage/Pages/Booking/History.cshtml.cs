using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;
using Service.Dtos;
using Service.Services.IServices;

namespace RazorPage.Pages.Booking
{
    public class HistoryModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public HistoryModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IList<BookingHistoryDto> BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = HttpContext.Session.GetObjectFromJson<UserDto>("User");

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            BookingReservation = await _bookingService.GetBookingByCusId(user.userId);

            return Page();
        }
    }
}

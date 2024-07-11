using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace RazorPage.Pages.Admin.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public DeleteModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingreservation = await _context.BookingReservations.FirstOrDefaultAsync(m => m.BookingReservationId == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingreservation = await _context.BookingReservations.FindAsync(id);
            if (bookingreservation != null)
            {
                BookingReservation = bookingreservation;
                _context.BookingReservations.Remove(BookingReservation);
                //bookingreservation.BookingStatus = 0;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

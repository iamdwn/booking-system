using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace RazorPage.Pages.Admin.Bookings
{
    public class EditModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public SelectList BookingStatusList { get; set; }

        public EditModel(Repository.Models.FuminiHotelManagementContext context)
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

            var bookingreservation =  await _context.BookingReservations.FirstOrDefaultAsync(m => m.BookingReservationId == id);
            if (bookingreservation == null)
            {
                return NotFound();
            }
            BookingReservation = bookingreservation;
            BookingStatusList = new SelectList(new List<int> { 0, 1 });
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "EmailAddress");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                BookingStatusList = new SelectList(new List<int> { 0, 1 });
                //return Page();
            }

            _context.Attach(BookingReservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingReservationExists(BookingReservation.BookingReservationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BookingReservationExists(int id)
        {
            return _context.BookingReservations.Any(e => e.BookingReservationId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Models;

namespace RazorPage.Pages.Admin.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public CreateModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "EmailAddress");
            return Page();
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BookingReservations.Add(BookingReservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

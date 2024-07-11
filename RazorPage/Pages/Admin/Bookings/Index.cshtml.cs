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
    public class IndexModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public IndexModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<BookingReservation> BookingReservation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BookingReservation = await _context.BookingReservations
                .Include(b => b.Customer).ToListAsync();
        }
    }
}

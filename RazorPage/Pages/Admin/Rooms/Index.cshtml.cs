using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace RazorPage.Pages.Admin.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public IndexModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<RoomInformation> RoomInformation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RoomInformation = await _context.RoomInformations
                .Include(r => r.RoomType).ToListAsync();
        }
    }
}

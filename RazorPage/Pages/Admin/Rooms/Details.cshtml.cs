﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace RazorPage.Pages.Admin.Rooms
{
    public class DetailsModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public DetailsModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public RoomInformation RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roominformation = await _context.RoomInformations.Include(r => r.RoomType).FirstOrDefaultAsync(m => m.RoomId == id);
            if (roominformation == null)
            {
                return NotFound();
            }
            else
            {
                RoomInformation = roominformation;
            }
            return Page();
        }
    }
}

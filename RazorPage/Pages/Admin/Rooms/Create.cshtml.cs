using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Models;

namespace RazorPage.Pages.Admin.Rooms
{
    public class CreateModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public SelectList RoomStatusList { get; set; }

        public CreateModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            RoomStatusList = new SelectList(new List<int> { 0, 1 });
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            return Page();
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RoomStatusList = new SelectList(new List<int> { 0, 1 });
                //return Page();
            }

            _context.RoomInformations.Add(RoomInformation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

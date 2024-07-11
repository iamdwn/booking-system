using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace RazorPage.Pages.Admin.Rooms
{
    public class EditModel : PageModel
    {
        private readonly Repository.Models.FuminiHotelManagementContext _context;

        public SelectList RoomStatusList { get; set; }

        public SelectList RoomTypeList { get; set; }

        public EditModel(Repository.Models.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roominformation =  await _context.RoomInformations.FirstOrDefaultAsync(m => m.RoomId == id);
            if (roominformation == null)
            {
                return NotFound();
            }
            RoomInformation = roominformation;
            //ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            RoomStatusList = new SelectList(new List<int> { 0, 1 });
            RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RoomStatusList = new SelectList(new List<int> { 0, 1 });
                RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
                //return Page();
            }

            _context.Attach(RoomInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomInformationExists(RoomInformation.RoomId))
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

        private bool RoomInformationExists(int id)
        {
            return _context.RoomInformations.Any(e => e.RoomId == id);
        }
    }
}

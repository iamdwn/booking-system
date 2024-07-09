using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;
using Service.Dtos;

namespace RazorPage.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public IndexModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<RoomInformation> Rooms { get; set; }

        [BindProperty]
        public List<RoomType> Types { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchDescription { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchCapacity { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? SearchPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchTypeId { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await _context.RoomInformations
                .Where(r => r.RoomStatus == (byte)1)
                .Include(r => r.RoomType)
                .ToListAsync();
            Types = await _context.RoomTypes.Where(t => true).ToListAsync();

            if (!string.IsNullOrEmpty(SearchDescription))
            {
                Rooms = Rooms.Where(r => r.RoomDetailDescription.Contains(SearchDescription, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (SearchCapacity.HasValue)
            {
                Rooms = Rooms.Where(r => r.RoomMaxCapacity == SearchCapacity.Value).ToList();
            }

            if (SearchPrice.HasValue)
            {
                Rooms = Rooms.Where(r => r.RoomPricePerDay <= SearchPrice.Value).ToList();
            }

            if (SearchTypeId.HasValue)
            {
                Rooms = Rooms.Where(r => r.RoomTypeId == SearchTypeId.Value).ToList();
            }
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int roomId)
        {
            var room = await _context.RoomInformations.FindAsync(roomId);

            if (room == null)
            {
                return NotFound();
            }

            // Add the room to the cart (store in session or database)
            var user = HttpContext.Session.GetObjectFromJson<UserDto>("User");
            if (user == null || !user.isAuthenticated) return RedirectToPage();

            var cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
            cart.Add(roomId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToPage();
        }
    }
}

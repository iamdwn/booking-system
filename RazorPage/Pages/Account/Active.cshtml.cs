using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Services.Interfaces;

namespace RazorPage.Pages.Account
{
    public class ActiveModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<ActiveModel> _logger;

        public ActiveModel(IUserService userService, ILogger<ActiveModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string email)
        {
            var user = await _userService.GetCustomer(c => c.EmailAddress.Equals(email));

            if (user == null)
            {
                return NotFound();
            }

            user.CustomerStatus = 1;

            var result = await _userService.UpdateCustomer(user);

            if (result)
            {
                return RedirectToPage("/Rooms/Index");
            }

            return RedirectToPage("Account/Login");
        }
    }
}

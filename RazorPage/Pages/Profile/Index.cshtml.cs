using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models;
using Service;
using Service.Dtos;
using Service.Services.IServices;

namespace RazorPage.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var customer = HttpContext.Session.GetObjectFromJson<UserDto>("User");

            Customer = await _userService.GetCustomerById(customer.userId);

            if (Customer == null)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userService.UpdateCustomer(Customer);

            if (result)
            {
                TempData["toast-success"] = "Update Profile success!";
            }
            else
            {
                TempData["toast-error"] = "Something went wrong!";
            }

            return RedirectToPage();
        }
    }
}

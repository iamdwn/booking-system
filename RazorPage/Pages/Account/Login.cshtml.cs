using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Dtos;
using Service.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(Input.Email, Input.Password);
                if (result != null)
                {
                    TempData["toast-success"] = "Login success!";

                    var user = new UserDto
                    {
                        isAuthenticated = true,
                        userId = result.CustomerId,
                        userName = result.CustomerFullName
                    };

                    //HttpContext.Session.SetInt32("UserId", result.CustomerId);
                    HttpContext.Session.SetObjectAsJson("User",user);
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    return RedirectToPage("/Rooms/Index");
                }
                else
                {
                    TempData["toast-error"] = "Login failed!";
                    //return Page();
                    return RedirectToPage("/Account/Login");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

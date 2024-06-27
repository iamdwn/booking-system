using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");

        if (string.IsNullOrEmpty(isAuthenticated))
        {
            context.Result = new RedirectToPageResult("/Account/Login");
        }
    }
}

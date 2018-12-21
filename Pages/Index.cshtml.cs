using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControlEnvRazor.Pages
{
    public class IndexModel : PageModel
    {
        public string accessToken { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                //accessToken = Request.Headers["Authorization"];
                accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(HttpContext, IdentityConstants.ExternalScheme, "access_token");
                //HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
                //accessToken = token;

            }
            return Page();
        }
    }
}

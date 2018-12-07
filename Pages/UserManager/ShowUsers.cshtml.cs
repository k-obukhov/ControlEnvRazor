using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ControlEnvRazor.Pages.UserManager
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class ShowUsersModel : PageModel
    {
        private readonly ApplicationDbContext context;
        public UserManager<ApplicationUser> UserManager { get; set; }

        public ShowUsersModel(ApplicationDbContext c, UserManager<ApplicationUser> m)
        {
            context = c;
            UserManager = m;
            applicationUsers = new List<ApplicationUser>();
        }

        public List<ApplicationUser> applicationUsers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            applicationUsers = await context.Users.ToListAsync();

            return Page();
        }
    }
}
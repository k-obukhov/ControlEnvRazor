using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControlEnvRazor.Pages.UserManager
{
    [Authorize(Roles=BaseInitializer.teacherName)]
    public class ChangeUserRolesModel : PageModel
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public ChangeUserRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

        public string UserId { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string UserId)
        {
            if (UserId == null)
                return NotFound();

            this.UserId = UserId;
            ApplicationUser user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound();

            UserRoles = await _userManager.GetRolesAsync(user);
            AllRoles = _roleManager.Roles.ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string UserId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                
                return RedirectToPage("./ShowUsers");
            }
            return NotFound();
        }
    }
}
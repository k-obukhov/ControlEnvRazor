using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlEnvRazor
{
    public class BaseInitializer
    {
        public const string adminName = "admin"; // на будущее !
        public const string studentName = "student";
        public const string teacherName = "teacher";

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(adminName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminName));
            }
            if (await roleManager.FindByNameAsync(studentName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(studentName));
            }
            if (await roleManager.FindByNameAsync(teacherName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(teacherName));
            }
            
            await userManager.AddToRolesAsync(userManager.Users.First(), new string[] { studentName, teacherName, adminName });
            
        }
    }
}

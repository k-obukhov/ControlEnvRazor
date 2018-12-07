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

namespace ControlEnvRazor.Pages.TaskManager
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class EditTaskModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        public EditTaskModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }

        [BindProperty]
        public UserTask userTask { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await manager.GetUserAsync(User);
            userTask = await context.UserTasks.SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

            if (userTask == null)
            {
                return NotFound();
            }
            return Page();
        }


        [Authorize(Roles = BaseInitializer.teacherName)]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await manager.GetUserAsync(User);
            var taskToUpdate = await context.UserTasks.SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

            if (await TryUpdateModelAsync<UserTask>(
                taskToUpdate,
                "userTask",
                t => t.TaskName, t => t.CommonDescription))
            {
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowTasks");
            }

            return Page();
        }
    }
}
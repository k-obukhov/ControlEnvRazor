using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ControlEnvRazor.Pages.TaskManager
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class EditVariantModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        public EditVariantModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }

        [BindProperty]
        public TaskVariant TaskVariant { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? taskID)
        {
            if (id == null || taskID == null)
            {
                return NotFound();
            }

            var user = await manager.GetUserAsync(User);
            TaskVariant = await context.TaskVariants
                            .SingleOrDefaultAsync(t => t.Id == id && t.UserTaskId == taskID && t.UserTask.ApplicationUserId == user.Id);

            if (TaskVariant == null)
            {
                return NotFound();
            }
            return Page();
        }


        [Authorize(Roles = BaseInitializer.teacherName)]
        public async Task<IActionResult> OnPostAsync(int id, int taskID)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await manager.GetUserAsync(User);
            var variantToUpdate = await context.TaskVariants
                            .SingleOrDefaultAsync(t => t.Id == id && t.UserTaskId == taskID && t.UserTask.ApplicationUserId == user.Id);

            if (TaskVariant == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<TaskVariant>(
                variantToUpdate,
                "taskVariant",
                t => t.NumberOfVariant, t => t.Description))
            {
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowVariants", new { id = taskID });
            }

            return Page();
        }
    }
}
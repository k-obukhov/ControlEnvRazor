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
    public class CreateVariantModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        [BindProperty]
        public TaskVariant TaskVariant { get; set; }

        public CreateVariantModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Для дефолтных данных!
            if (id == null)
                return NotFound();

            var user = await manager.GetUserAsync(User);
            var taskToUpdate = await context.UserTasks.SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

            if (taskToUpdate == null)
                return NotFound();

            TaskVariant = new TaskVariant();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyEntry = new TaskVariant();
            if (await TryUpdateModelAsync<TaskVariant>(
                emptyEntry,
                "taskVariant", // prefix
                t => t.NumberOfVariant, t => t.Description))
            {
                var user = await manager.GetUserAsync(User);
                var taskToUpdate = await context.UserTasks.SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

                emptyEntry.UserTask = taskToUpdate;

                context.TaskVariants.Add(emptyEntry);
                
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowVariants", new { id });
            }
            return null;
        }
    }
}
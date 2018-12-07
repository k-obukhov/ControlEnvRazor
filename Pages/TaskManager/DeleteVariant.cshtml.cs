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
    public class DeleteVariantModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        public DeleteVariantModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }

        [BindProperty]
        public TaskVariant TaskVariant { get; set; }
        public string errorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? taskID, bool? saveChangesError = false)
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

            if (saveChangesError.GetValueOrDefault())
            {
                errorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? taskID)
        {
            if (id == null || taskID == null)
            {
                return NotFound();
            }

            var user = await manager.GetUserAsync(User);
            var foundVar = await context.TaskVariants
                            .SingleOrDefaultAsync(t => t.Id == id && t.UserTaskId == taskID && t.UserTask.ApplicationUserId == user.Id);

            if (foundVar == null)
            {
                return NotFound();
            }

            try
            {
                context.TaskVariants.Remove(foundVar);
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowVariants", new { id = taskID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./DeleteVariant",
                                     new { id, taskID, saveChangesError = true });
            }
        }
    }
}
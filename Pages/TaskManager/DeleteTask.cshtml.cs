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

namespace ControlEnvRazor.Pages
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class DeleteTaskModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        public DeleteTaskModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }

        [BindProperty]
        public UserTask userTask { get; set; }
        public string errorMessage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await manager.GetUserAsync(User);
            userTask = await context.UserTasks
                            .SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

            if (userTask == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                errorMessage = "Delete failed. Try again";
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await manager.GetUserAsync(User);
            var foundTask = await context.UserTasks
                            .SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == user.Id);

            if (foundTask == null)
            {
                return NotFound();
            }

            try
            {
                context.UserTasks.Remove(foundTask);
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowTasks");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./DeleteTask",
                                     new { id, saveChangesError = true });
            }
        }

    }
}
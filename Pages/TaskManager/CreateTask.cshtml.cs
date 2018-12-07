using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ControlEnvRazor.Pages.TaskManager
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class CreateTaskModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        [BindProperty]
        public UserTask userTask { get; set; }

        public CreateTaskModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            context = dbContext;
            manager = userManager;
        }
        
        public IActionResult OnGet()
        {
            // Для дефолтных данных!
            userTask = new UserTask();
            /*
            {
                TaskName = "Задание 1",
                CommonDescription = "Описание для задания"
            };
            */
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyEntry = new UserTask();
            if (await TryUpdateModelAsync<UserTask>(
                emptyEntry,
                "userTask", // prefix
                t => t.TaskName, t => t.CommonDescription))
            {
                context.UserTasks.Add(emptyEntry);

                var user = await manager.GetUserAsync(User);

                emptyEntry.ApplicationUser = user;
    
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowTasks");
            }
            return null;
        }
    }
}
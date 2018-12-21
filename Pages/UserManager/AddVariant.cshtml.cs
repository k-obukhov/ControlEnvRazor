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
    public struct JSONVariant
    {
        // Структура для варианта в JSON
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
    }

    [Authorize(Roles=BaseInitializer.teacherName)]
    public class AddVariantModel : PageModel
    {
        //public string UserId { get; set; }
        private ApplicationDbContext context;
        private UserManager<ApplicationUser> manager;

        public List<UserTask> UserTasks { get; set; }
        public List<TaskVariant> TaskVariants { get; set; }

        public AddVariantModel(ApplicationDbContext c, UserManager<ApplicationUser> m)
        {
            context = c;
            manager = m;

            UserTasks = new List<UserTask>();
            TaskVariants = new List<TaskVariant>();
        }

        public async Task<IActionResult> OnGetAsync(string UserId)
        {
            var currentUser = await manager.GetUserAsync(User);

            UserTasks = await (from elem in context.UserTasks where elem.ApplicationUserId == currentUser.Id select elem).ToListAsync();
            //this.UserId = UserId;

            TaskVariants = await (from elem in context.TaskVariants where elem.UserTaskId == UserTasks.FirstOrDefault().Id select elem).ToListAsync();

            return Page();
        }

        public JsonResult OnGetTaskVariants(int id)
        {
            TaskVariants = (from elem in context.TaskVariants where elem.UserTaskId == id select elem).ToList();

            List<JSONVariant> vars = new List<JSONVariant>();

            foreach (var elem in TaskVariants)
                vars.Add(new JSONVariant { Id = elem.Id, Number = elem.NumberOfVariant, Description = elem.Description });

            return new JsonResult(vars);
        }

        public async Task<IActionResult> OnPostAsync(string UserId, int selectedVariant)
        {
            var curUser = await manager.GetUserAsync(User);
            var taskVar = await context.TaskVariants.FirstOrDefaultAsync(el => el.UserTask.ApplicationUserId == curUser.Id && el.Id == selectedVariant);
            var selectedUser = await manager.FindByIdAsync(UserId);

            if (taskVar == null || selectedUser == null)
                return NotFound();

            
            await context.UserVariants.AddAsync(new UserVariant { TaskVariant = taskVar, ApplicationUser = selectedUser });
            await context.SaveChangesAsync();
            return RedirectToPage("./ShowUsers");
        }
        
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ControlEnvRazor.Pages.TaskManager
{
    [Authorize(Roles = BaseInitializer.teacherName)]
    public class ShowTasksModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<UserTask> userTasks { get; set; }

        public ShowTasksModel(ApplicationDbContext c)
        {
            context = c;
            userTasks = new List<UserTask>();
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            this.userTasks = await (from userTask in context.UserTasks
                                    where userTask.ApplicationUser.UserName == User.Identity.Name
                                    select userTask).ToListAsync();
            return Page();
        }

    }
}
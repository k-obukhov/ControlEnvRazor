using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ControlEnvRazor.Pages.TaskManager
{
    [Authorize(Roles=BaseInitializer.teacherName)]
    public class ShowVariantsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<TaskVariant> TaskVariants { get; set; }
        public int? currentID { get; set; }

        public ShowVariantsModel(ApplicationDbContext dbContext)
        {
            context = dbContext;
            TaskVariants = new List<TaskVariant>();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();
            TaskVariants = await (from TaskVar in context.TaskVariants
                                  where TaskVar.UserTask.Id == id
                                  select TaskVar).ToListAsync();
            this.currentID = id;
            return Page();
        }
    }
}
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
    [Authorize(Roles=BaseInitializer.teacherName)]
    public class EditUserVariantModel : PageModel
    {
        // Редактирование и удаление варианта пользовательского решения
        // Редактируется только статус решения студента (а может вообще его убрать?)
        // Удаляется весь вариант по запросу

        private ApplicationDbContext context;
        private UserManager<ApplicationUser> manager;

        public EditUserVariantModel(ApplicationDbContext c, UserManager<ApplicationUser> m)
        {
            context = c;
            manager = m;
        }

        [BindProperty]
        public UserVariant Variant { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var curUser = await manager.GetUserAsync(User);
            Variant = await context.UserVariants.FirstOrDefaultAsync(el => el.Id == id && el.TaskVariant.UserTask.ApplicationUserId == curUser.Id);
            if (Variant == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curUser = await manager.GetUserAsync(User);
            Variant = await context.UserVariants.FirstOrDefaultAsync(el => el.Id == id && el.TaskVariant.UserTask.ApplicationUserId == curUser.Id);


            if (Variant == null)
            {
                return NotFound();
            }

            try
            {
                context.UserVariants.Remove(Variant);
                await context.SaveChangesAsync();
                return RedirectToPage("./ShowUsers");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./DeleteTask",
                                     new { id });
            }
        }

    }
}
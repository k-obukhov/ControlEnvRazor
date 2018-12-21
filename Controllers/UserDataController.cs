using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlEnvRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ControlEnvRazor.Controllers
{
    [Route("api/[controller]")]
    public class UserDataController : Controller
    {

        private UserManager<ApplicationUser> UserManager { get; set; }
        private ApplicationDbContext Context { get; set; }

        public UserDataController(UserManager<ApplicationUser> manager, ApplicationDbContext context)
        {
            UserManager = manager;
            Context = context;
        }

        [Authorize]
        [HttpGet("/GetTasks")]
        public async Task GetTasks()
        {
            var curUser = await UserManager.GetUserAsync(User);
            var variants = Context.UserVariants.Where(e => e.ApplicationUserId == curUser.Id);

            var response = new List<object>();
            foreach (var variant in variants)
            {
                response.Add(
                    new
                    {
                        TaskVariantId = variant.TaskVariantId,
                        TaskVariantDescription = variant.TaskVariant.Description,
                        TaskVariantNumber = variant.TaskVariant.NumberOfVariant,

                        TaskId = variant.TaskVariant.UserTask.Id,
                        TaskName = variant.TaskVariant.UserTask.TaskName,
                        TaskDescription = variant.TaskVariant.UserTask.CommonDescription,

                        TaskLink = variant.GithubRepo
                    });
            }

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response.ToArray(), new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [Authorize]
        [HttpPost("/SendLink")]
        public async Task SendLink(int Id, string Link)
        {
            var curUser = await UserManager.GetUserAsync(User);
            var variant = await Context.UserVariants.FirstOrDefaultAsync(e => e.Id == Id && e.ApplicationUserId == curUser.Id);

            if (variant == null || curUser == null)
            {
                // Можно и ссылку на гитхаб проверять
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid TaskId");
                return;
            }

            variant.GithubRepo = Link;
            Context.Update(variant);
            await Context.SaveChangesAsync();

            Response.StatusCode = 200;
            await Response.WriteAsync("Send successfully");
        }
    }
}
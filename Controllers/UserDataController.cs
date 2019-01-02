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

        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize]
        [Route("gettasks")]
        public async Task GetTasks()
        {
            var curUserId = User.Identity.Name; // userid
            if (curUserId != null)
            {
                var variants = Context.UserVariants.Where(e => e.ApplicationUserId == curUserId);

                var response = new List<object>();
                if (variants != null)
                {
                    foreach (var variant in variants)
                    {
                        response.Add(
                        new
                        {
                            VariantId = variant.Id,

                            TaskVariantId = variant.TaskVariantId,
                            TaskVariantDescription = variant.TaskVariant.Description,
                            TaskVariantNumber = variant.TaskVariant.NumberOfVariant,

                            TaskId = variant.TaskVariant.UserTask.Id,
                            TaskName = variant.TaskVariant.UserTask.TaskName,
                            TaskDescription = variant.TaskVariant.UserTask.CommonDescription,

                            TaskLink = variant.GithubRepo
                        });
                    }
                }

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(response.ToArray(), new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("sendlink")]
        public async Task SendLink(int Id, string Link)
        {
            var variant = await Context.UserVariants.FirstOrDefaultAsync(e => e.Id == Id && e.ApplicationUserId == User.Identity.Name);

            if (variant == null )
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
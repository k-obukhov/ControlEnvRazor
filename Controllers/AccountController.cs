using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Microsoft.AspNetCore.Identity;
using ControlEnvRazor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlEnvRazor.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }
        private IConfiguration Config { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }

        public AccountController(ApplicationDbContext dbContext, IConfiguration conf, UserManager<ApplicationUser> manager)
        {
            DbContext = dbContext;
            Config = conf;
            UserManager = manager;
        }

        private async Task<ApplicationUser> GetUser(string githubName, string githubPassword)
        {
            // Хранения ролей и прочего не происходит
            // Прототип!

            var client = new GitHubClient(new ProductHeaderValue("ControlEnvRazor"));
            var basicAuth = new Credentials(githubName, githubPassword);

            client.Credentials = basicAuth;
            
            try
            {
                var user = await client.User.Current();
                int id = user.Id;

                var userSystem = await DbContext.UserLogins.FirstOrDefaultAsync(elem => elem.ProviderKey == id.ToString() && elem.LoginProvider == "GitHub");

                if (userSystem != null)
                {
                    return await DbContext.Users.FirstOrDefaultAsync(elem => elem.Id == userSystem.UserId);
                }
            }
            catch (AuthorizationException)
            {
                return null;
            }
        
            return null;
        }

        [HttpPost("/token")]
        public async Task Token(string username, string password)
        {
            var user = await GetUser(username, password);

            if (user == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: await UserManager.GetClaimsAsync(user),
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = user.UserName,
                id = user.Id
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

        }


    }
}

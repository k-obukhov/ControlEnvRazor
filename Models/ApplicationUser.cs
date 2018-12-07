using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ControlEnvRazor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<UserVariant> UserVariants { get; set; }
        public virtual List<UserTask> UserTasks { get; set; }

        public string GithubLogin { get; set; }

        public ApplicationUser()
        {
            this.UserVariants = new List<UserVariant>();
            this.UserTasks = new List<UserTask>();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Notification>? Notifications { get; set; }
        public List<CustomTask>? CustomTasks { get; set; }
    }
}

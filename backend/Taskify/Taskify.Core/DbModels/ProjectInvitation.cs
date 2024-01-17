using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class ProjectInvitation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Notification Notification { get; set; }
        public Project Project { get; set; }
        public bool? IsAccepted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class ProjectMember
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public User User { get; set; }
        public ProjectRole ProjectRole { get; set; }
    }
}

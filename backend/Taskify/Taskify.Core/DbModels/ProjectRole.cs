using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.DbModels
{
    public class ProjectRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public string Name { get; set; }
        public ProjectRoleType ProjectRoleType { get; set; }
    }
}

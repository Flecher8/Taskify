using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class Project
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Section> Sections { get; set; }
        public List<ProjectInvitation> ProjectInvitations { get; set; }
        public List<ProjectMember> ProjectMembers { get; set; }
        public List<ProjectRole> ProjectRoles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class ProjectDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }
        public UserDto User { get; set; }
        public int NormalWorkingHoursPerDay { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SectionDto>? Sections { get; set; }
        public List<ProjectMemberDto>? ProjectMembers { get; set; }
        public List<ProjectRoleDto>? ProjectRoles { get; set; }
    }
}

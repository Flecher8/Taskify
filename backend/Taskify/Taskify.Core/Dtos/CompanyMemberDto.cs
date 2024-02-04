using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CompanyMemberDto
    {
        public string Id { get; set; } = string.Empty;
        public UserDto User { get; set; }
        public CompanyDto Company { get; set; }
        public CompanyMemberRoleDto? Role { get; set; }
        public double Salary { get; set; }
    }
}

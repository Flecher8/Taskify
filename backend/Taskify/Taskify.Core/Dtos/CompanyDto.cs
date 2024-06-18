using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CompanyDto
    {
        public string Id { get; set; } = string.Empty;
        public UserDto User { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CompanyExpenseDto> CompanyExpenses { get; set; }
        public List<CompanyMemberDto> CompanyMembers { get; set; }
        public List<CompanyRoleDto> CompanyRoles { get; set; }
        public List<CompanyInvitationDto> CompanyInvitations { get; set; }
    }
}

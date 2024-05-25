using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public string Name { get; set; }
        public List<CompanyExpense> CompanyExpenses { get; set; }
        public List<CompanyMember> CompanyMembers { get; set; }
        public List<CompanyRole> CompanyRoles { get; set; }
        public List<CompanyInvitation> CompanyInvitations { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class CompanyMemberRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Company Company { get; set; }
        public string Name { get; set; }
        public List<CompanyMember> CompanyMembers { get; set; }
    }
}

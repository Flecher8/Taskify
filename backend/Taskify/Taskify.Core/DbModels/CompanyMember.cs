using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class CompanyMember
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public Company Company { get; set; }
        public CompanyMemberRole? Role { get; set; }
        public double Salary { get; set; }
    }
}

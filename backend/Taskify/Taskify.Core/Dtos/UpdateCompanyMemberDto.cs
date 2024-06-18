using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class UpdateCompanyMemberDto
    {
        public string Id { get; set; } = string.Empty;
        public string? RoleId { get; set; } = string.Empty;
        public double Salary { get; set; }
    }
}

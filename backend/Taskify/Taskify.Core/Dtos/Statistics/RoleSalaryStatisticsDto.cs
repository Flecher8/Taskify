using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos.Statistics
{
    public class RoleSalaryStatisticsDto
    {
        public CompanyRoleDto? Role { get; set; }
        public double TotalSalary { get; set; }
    }
}

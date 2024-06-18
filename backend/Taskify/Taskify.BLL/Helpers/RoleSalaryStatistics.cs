using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Helpers
{
    public class RoleSalaryStatistics
    {
        public CompanyRole? Role { get; set; }
        public double TotalSalary { get; set; }
    }
}

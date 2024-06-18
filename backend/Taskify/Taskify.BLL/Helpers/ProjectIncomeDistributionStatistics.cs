using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Helpers
{
    public class ProjectIncomeDistributionStatistics
    {
        public Project Project { get; set; }
        public double TotalIncome { get; set; }
    }
}

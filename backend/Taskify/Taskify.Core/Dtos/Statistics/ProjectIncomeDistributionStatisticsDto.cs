using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos.Statistics
{
    public class ProjectIncomeDistributionStatisticsDto
    {
        public ProjectDto Project { get; set; }
        public double TotalIncome { get; set; }
    }
}

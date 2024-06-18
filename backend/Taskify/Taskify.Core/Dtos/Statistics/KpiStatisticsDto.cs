using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos.Statistics
{
    public class KpiStatisticsDto
    {
        public UserDto User { get; set; }
        public double Kpi { get; set; }
    }
}

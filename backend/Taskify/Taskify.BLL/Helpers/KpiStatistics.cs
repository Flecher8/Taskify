using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Helpers
{
    public class KpiStatistics
    {
        public User User { get; set; }
        public double Kpi { get; set; }
    }
}

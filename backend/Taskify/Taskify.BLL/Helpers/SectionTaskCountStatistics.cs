using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Helpers
{
    public class SectionTaskCountStatistics
    {
        public Section Section { get; set; }
        public int Count { get; set; }
    }
}

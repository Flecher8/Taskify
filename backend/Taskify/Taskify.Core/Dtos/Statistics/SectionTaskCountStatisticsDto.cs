using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos.Statistics
{
    public class SectionTaskCountStatisticsDto
    {
        public SectionDto Section { get; set; }
        public int Count { get; set; }
    }
}

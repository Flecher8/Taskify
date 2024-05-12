using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos.Statistics
{
    public class SectionTypeTaskCountStatisticsDto
    {
        public SectionType SectionType { get; set; }
        public int Count { get; set; }
    }
}

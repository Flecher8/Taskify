using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos.Statistics
{
    public class ProjectRoleTaskCountStatisticsDto
    {
        public ProjectRole? ProjectRole { get; set; }
        public int Count { get; set; }
    }
}

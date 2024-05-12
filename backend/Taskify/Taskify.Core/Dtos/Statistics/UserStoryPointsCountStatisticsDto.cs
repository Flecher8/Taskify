using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos.Statistics
{
    public class UserStoryPointsCountStatisticsDto
    {
        public User? User { get; set; }
        public int Count { get; set; }
    }
}

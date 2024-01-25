using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class SubscriptionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PricePerMonth { get; set; }
        public int DurationInDays { get; set; }
        public int ProjectsLimit { get; set; }
        public int ProjectMembersLimit { get; set; }
        public int ProjectSectionsLimit { get; set; }
        public int ProjectTasksLimit { get; set; }
    }
}

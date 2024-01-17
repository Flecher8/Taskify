using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class Subscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerMonth { get; set; }
        public int DurationInDays { get; set; }
        public int ProjectsLimit { get; set; }
        public int ProjectMembersLimit { get; set; }
        public int ProjectSectionsLimit { get; set; }
        public int ProjectTasksLimit { get; set; }
    }
}

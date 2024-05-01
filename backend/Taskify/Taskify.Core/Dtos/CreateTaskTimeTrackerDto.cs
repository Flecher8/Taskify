using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class CreateTaskTimeTrackerDto
    {
        public string UserId { get; set; } = string.Empty;
        public string CustomTaskId { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

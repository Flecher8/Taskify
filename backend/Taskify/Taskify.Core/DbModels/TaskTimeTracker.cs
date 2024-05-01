using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class TaskTimeTracker
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public CustomTask CustomTask { get; set; }
        public User User { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint DurationInSeconds { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

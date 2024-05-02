using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos
{
    public class TaskTimeTrackerDto
    {
        public string Id { get; set; } = string.Empty;
        public CustomTaskDto CustomTask { get; set; }
        public UserDto User { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint DurationInSeconds { get; set; }
        public string Description { get; set; } = string.Empty;
        public TaskTimeTrackerType TrackerType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CustomTaskDto
    {
        public string Id { get; set; } = string.Empty;
        public SectionWithoutTasksDto? Section { get; set; }
        public UserDto? ResponsibleUser { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDateTimeUtc { get; set; }
        public DateTime? EndDateTimeUtc { get; set; }
        public int? StoryPoints { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SequenceNumber { get; set; }
        public List<TaskTimeTrackerDto> TaskTimeTrackers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class CustomTask
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Section Section { get; set; }
        public User? ResponsibleUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTimeUtc { get; set; }
        public DateTime? EndDateTimeUtc { get; set; }
        public int? StoryPoints { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SequenceNumber { get; set; }
        public List<TaskTimeTracker> TaskTimeTrackers { get; set; }
    }
}

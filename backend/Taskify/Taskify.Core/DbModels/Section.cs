using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.DbModels
{
    public class Section
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SequenceNumber { get; set; }
        public SectionType SectionType { get; set; }
        public bool IsArchived { get; set; }
        public List<CustomTask>? CustomTasks { get; set; }
    }
}

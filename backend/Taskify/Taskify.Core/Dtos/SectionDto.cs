using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos
{
    public class SectionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ProjectDto Project { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SequenceNumber { get; set; }
        public SectionType SectionType { get; set; }
        public bool IsArchived { get; set; }
        public List<CustomTask>? CustomTasks { get; set; }
    }
}

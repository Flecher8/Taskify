using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class RedirectCustomTaskDto
    {
        public required string Id { get; set; }
        public required string TargetSectionId { get; set; }
        public int? TargetSequenceNumber { get; set; }
    }
}

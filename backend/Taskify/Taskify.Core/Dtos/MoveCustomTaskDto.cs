using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class MoveCustomTaskDto
    {
        public required string Id { get; set; }
        public required int TargetSequenceNumber {get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class MoveSectionDto
    {
        public string Id { get; set; } = string.Empty;
        public int MoveTo { get; set; }
    }
}

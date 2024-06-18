using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class CreateProjectMemberDto
    {
        public string ProjectId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; 
    }
}

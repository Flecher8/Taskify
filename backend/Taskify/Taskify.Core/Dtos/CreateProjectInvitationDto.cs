using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CreateProjectInvitationDto
    {
        public string ProjectId { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class ProjectInvitationDto
    {
        public string Id { get; set; } = string.Empty;
        public NotificationDto Notification { get; set; }
        public ProjectDto Project { get; set; }
        public bool? IsAccepted { get; set; }
    }
}

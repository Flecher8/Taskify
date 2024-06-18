using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CompanyInvitationDto
    {
        public string Id { get; set; } = string.Empty;
        public NotificationDto Notification { get; set; }
        public CompanyDto Company { get; set; }
        public bool? IsAccepted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
    }
}

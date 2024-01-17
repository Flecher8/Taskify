using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class CompanyInvitation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Notification Notification { get; set; }
        public Company Company { get; set; }
        public bool? IsAccepted { get; set; }
    }
}

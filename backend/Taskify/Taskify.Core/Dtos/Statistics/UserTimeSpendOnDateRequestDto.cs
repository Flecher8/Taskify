using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos.Statistics
{
    public class UserTimeSpendOnDateRequestDto
    {
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public DateTime Date { get; set; }
    }
}

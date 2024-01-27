using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.DbModels
{
    public class ProjectIncome
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public double Amount { get; set; }
        public ProjectIncomeFrequency Frequency { get; set; }
    }
}

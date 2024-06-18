using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos
{
    public class ProjectIncomeDto
    {
        public string Id { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Name { get; set; }
        public ProjectIncomeFrequency Frequency { get; set; }
    }
}

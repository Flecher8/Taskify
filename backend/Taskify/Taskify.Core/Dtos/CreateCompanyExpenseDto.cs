using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.Core.Dtos
{
    public class CreateCompanyExpenseDto
    {
        public string CompanyId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public CompanyExpenseFrequency Frequency { get; set; }
    }
}

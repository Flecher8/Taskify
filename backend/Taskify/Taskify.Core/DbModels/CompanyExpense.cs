using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.DbModels
{
    public class CompanyExpense
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Company Company { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public CompanyExpenseFrequency Frequency { get; set; }
    }
}

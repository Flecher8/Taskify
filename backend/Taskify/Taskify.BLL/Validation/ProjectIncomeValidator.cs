using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class ProjectIncomeValidator : BaseValidator<ProjectIncome>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(ProjectIncome projectIncome)
        {
            var baseResult = await base.ValidateAsync(projectIncome);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(projectIncome.Id, out _))
            {
                errorMessages.Add("Invalid project income Id format.");
            }

            if (projectIncome.Amount < 0)
            {
                errorMessages.Add("Amount can not be less than 0.");
            }

            if (!Enum.IsDefined(typeof(ProjectIncomeFrequency), projectIncome.Frequency))
            {
                errorMessages.Add("Invalid project income frequency.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

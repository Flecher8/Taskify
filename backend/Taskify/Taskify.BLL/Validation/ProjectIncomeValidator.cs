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
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(ProjectIncome entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid project income Id format.");
            }

            if (entity.Amount <= 0)
            {
                errorMessages.Add("Amount can not be less than 0.");
            }

            if (!Enum.IsDefined(typeof(ProjectIncomeFrequency), entity.Frequency))
            {
                errorMessages.Add("Invalid project income frequency.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

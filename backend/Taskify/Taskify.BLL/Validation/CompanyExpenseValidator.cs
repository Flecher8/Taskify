using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class CompanyExpenseValidator : BaseValidator<CompanyExpense>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(CompanyExpense entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid company expense Id format.");
            }

            if (entity.Amount < 0)
            {
                errorMessages.Add("Amount can not be less than 0.");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errorMessages.Add("Company expense name can not be null or empty.");
            }

            if (!Enum.IsDefined(typeof(CompanyExpenseFrequency), entity.Frequency))
            {
                errorMessages.Add("Invalid company expense frequency.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

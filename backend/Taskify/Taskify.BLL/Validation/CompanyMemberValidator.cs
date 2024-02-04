using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Validation
{
    public class CompanyMemberValidator : BaseValidator<CompanyMember>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(CompanyMember entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid company member Id format.");
            }

            if (entity.Salary <= 0)
            {
                errorMessages.Add("Salary can not be less than 0.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

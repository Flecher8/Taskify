using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class CompanyMemberRoleValidator : BaseValidator<CompanyMemberRole>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(CompanyMemberRole entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid company member role Id format.");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errorMessages.Add("Company member role name can not be null or empty.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

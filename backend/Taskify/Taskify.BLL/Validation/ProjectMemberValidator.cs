using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class ProjectMemberValidator : BaseValidator<ProjectMember>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(ProjectMember entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid project member Id format.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

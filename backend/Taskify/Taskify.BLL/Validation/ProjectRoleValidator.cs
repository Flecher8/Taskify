using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class ProjectRoleValidator : BaseValidator<ProjectRole>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(ProjectRole entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid project role Id format.");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errorMessages.Add("Project role name cannot be null or empty.");
            }

            if (!Enum.IsDefined(typeof(ProjectRoleType), entity.ProjectRoleType))
            {
                errorMessages.Add("Invalid project role type.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

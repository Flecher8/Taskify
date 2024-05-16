using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Migrations;

namespace Taskify.BLL.Validation
{
    public class ProjectValidator : BaseValidator<Project>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(Project entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid project Id format.");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errorMessages.Add("Project name cannot be null or empty.");
            }

            if (entity.NormalWorkingHoursPerDay < 0 || entity.NormalWorkingHoursPerDay > 24)
            {
                errorMessages.Add("Project normal working hours per day can not be smaller than 0 hours or bigger than 24 hours.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

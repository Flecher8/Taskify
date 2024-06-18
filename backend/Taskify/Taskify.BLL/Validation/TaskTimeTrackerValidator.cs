using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class TaskTimeTrackerValidator : BaseValidator<TaskTimeTracker>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(TaskTimeTracker entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid Id format.");
            }

            var minimalDate = new DateTime(2000, 01, 01);

            if (entity.StartDateTime < new DateTime(minimalDate.Ticks))
            {
                errorMessages.Add("Start date must be greater than or equal to January 1, 2000.");
            }
            
            if (entity.EndDateTime != null && entity.StartDateTime > entity.EndDateTime)
            {
                errorMessages.Add("End date must be greater than start date.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

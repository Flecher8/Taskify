using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.BLL.Validation
{
    public class CustomTaskValidator : BaseValidator<CustomTask>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(CustomTask entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid custom task Id format.");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errorMessages.Add("Custom task name cannot be null or empty.");
            }

            if (entity.StoryPoints != null && (entity.StoryPoints <= 0 || entity.StoryPoints >= 100))
            {
                errorMessages.Add("StoryPoints must be greater than 0, or null, or less than 100.");
            }

            if (entity.SequenceNumber < 0)
            {
                errorMessages.Add("SequenceNumber must be greater than or equal to 0.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

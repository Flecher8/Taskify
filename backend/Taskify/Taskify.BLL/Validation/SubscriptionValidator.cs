using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Validation
{
    public class SubscriptionValidator : Validator<Subscription>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(Subscription subscription)
        {
            var baseResult = await base.ValidateAsync(subscription);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            // Specific validation logic for Subscription entity

            // Check if Id is a valid Guid
            if (!Guid.TryParse(subscription.Id, out _))
            {
                errorMessages.Add("Id must be a valid Guid string.");
            }

            // Name validation
            if (string.IsNullOrEmpty(subscription.Name))
            {
                errorMessages.Add("Name cannot be null or empty.");
            }
            else if (subscription.Name.Length < 3 || subscription.Name.Length > 50)
            {
                errorMessages.Add("Name length must be between 3 and 50 characters.");
            }

            // PricePerMonth validation
            if (subscription.PricePerMonth < 0)
            {
                errorMessages.Add("Price per month must be greater than 0.");
            }

            // Additional validations for PricePerMonth, DurationInDays, ProjectsLimit, 
            // ProjectMembersLimit, ProjectSectionsLimit, and ProjectTasksLimit
            ValidatePositiveInteger("Duration in days", subscription.DurationInDays, errorMessages);
            ValidatePositiveInteger("Projects limit", subscription.ProjectsLimit, errorMessages);
            ValidatePositiveInteger("Project members limit", subscription.ProjectMembersLimit, errorMessages);
            ValidatePositiveInteger("Project sections limit", subscription.ProjectSectionsLimit, errorMessages);
            ValidatePositiveInteger("Project tasks limit", subscription.ProjectTasksLimit, errorMessages);

            return (errorMessages.Count == 0, errorMessages);
        }

        private void ValidatePositiveInteger(string propertyName, int value, List<string> errorMessages)
        {
            if (value <= 0)
            {
                errorMessages.Add($"{propertyName} must be greater than 0.");
            }
        }
    }
}

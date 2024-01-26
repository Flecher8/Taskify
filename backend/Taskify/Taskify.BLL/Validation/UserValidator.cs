using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Migrations;

namespace Taskify.BLL.Validation
{
    public class UserValidator : Validator<User>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(User user)
        {
            var baseResult = await base.ValidateAsync(user);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            // Specific validation logic for User entity

            // Check if Id is a valid Guid
            if (!Guid.TryParse(user.Id, out _))
            {
                errorMessages.Add("Id must be a valid Guid string.");
            }

            // Name validation
            if (string.IsNullOrEmpty(user.FirstName))
            {
                errorMessages.Add("First name cannot be null or empty.");
            }

            // Name validation
            if (string.IsNullOrEmpty(user.LastName))
            {
                errorMessages.Add("Last name cannot be null or empty.");
            }

            if (!IsDateTimeCorrect(user.CreatedAt))
            {
                errorMessages.Add("Creation date and time are not correct.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }

        private bool IsDateTimeCorrect(DateTime date)
        {
            // Constant
            DateTime CompanyCreationDate = new DateTime(2024, 1, 1);
            return date >= CompanyCreationDate && date <= DateTime.UtcNow.AddHours(1);
        }
    }
}

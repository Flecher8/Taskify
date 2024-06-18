using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class NotificationValidator : BaseValidator<Notification>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(Notification entity)
        {
            var baseResult = await base.ValidateAsync(entity);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var errorMessages = baseResult.ErrorMessages;

            if (!Guid.TryParse(entity.Id, out _))
            {
                errorMessages.Add("Invalid notification Id format.");
            }

            if (!Enum.IsDefined(typeof(NotificationType), entity.NotificationType))
            {
                errorMessages.Add("Invalid notification type.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

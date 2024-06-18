using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.BLL.Validation
{
    public class SectionValidator : BaseValidator<Section>
    {
        public override async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(Section entity)
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
                errorMessages.Add("Section name cannot be null or empty.");
            }

            if (entity.SequenceNumber < 0)
            {
                errorMessages.Add("Section sequence number can not be less than 0.");
            }


            if (!Enum.IsDefined(typeof(SectionType), entity.SectionType))
            {
                errorMessages.Add("Invalid section type.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

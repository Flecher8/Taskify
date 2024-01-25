using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.BLL.Validation
{
    public class Validator<T> : IValidator<T>
    {
        public virtual async Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(T entity)
        {
            var errorMessages = new List<string>();

            // Common validation logic goes here
            if (entity == null)
            {
                errorMessages.Add("Entity cannot be null.");
            }

            return (errorMessages.Count == 0, errorMessages);
        }
    }
}

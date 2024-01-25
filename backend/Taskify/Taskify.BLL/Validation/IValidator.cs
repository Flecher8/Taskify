using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.BLL.Validation
{
    public interface IValidator<T>
    {
        Task<(bool IsValid, List<string> ErrorMessages)> ValidateAsync(T entity);
    }
}


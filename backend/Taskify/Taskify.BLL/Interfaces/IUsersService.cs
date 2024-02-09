using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<Result<User>> GetUserByIdAsync(string id);

        Task<Result<List<User>>> GetAllUsersAsync();

        Task<Result<bool>> UpdateUserAsync(User user);

        Task<Result<bool>> DeleteUserAsync(string id);

        Task<Result<User>> GetUserByEmailAsync(string email);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IUserPermissionService
    {
        Task<Result<bool>> IsUserProjectMemberAsync(string userId, string projectId);
        Task<Result<bool>> IsUserProjectOwnerAsync(string userId, string projectId);
        Task<Result<bool>> IsUserProjectRoleAdminAsync(string userId, string projectId);
        Task<Result<bool>> IsUserProjectRoleGuestAsync(string userId, string projectId);
        Task<Result<bool>> IsUserProjectRoleMemberAsync(string userId, string projectId);
    }
}

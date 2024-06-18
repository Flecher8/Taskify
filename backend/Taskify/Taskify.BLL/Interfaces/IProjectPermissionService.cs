using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectPermissionService
    {
        Task<Result<bool>> CanViewProjectAsync(string userId, string projectId);
        Task<Result<bool>> CanEditProjectAsync(string userId, string projectId);
        Task<Result<bool>> CanEditProjectSettingsAsync(string userId, string projectId);
    }
}

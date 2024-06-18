using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectRolesService
    {
        Task<Result<ProjectRole>> CreateProjectRoleAsync(ProjectRole projectRole);

        Task<Result<ProjectRole>> GetProjectRoleByIdAsync(string id);

        Task<Result<List<ProjectRole>>> GetRolesByProjectIdAsync(string projectId);

        Task<Result<bool>> DeleteProjectRoleAsync(string id);

        Task<Result<bool>> UpdateProjectRoleAsync(ProjectRole projectRole);
    }
}

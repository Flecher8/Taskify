using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectMembersService
    {
        Task<Result<ProjectMember>> CreateProjectMemberAsync(ProjectMember projectMember);

        Task<Result<bool>> DeleteProjectMemberAsync(string id);

        Task<Result<bool>> UpdateProjectMemberAsync(ProjectMember projectMember);

        Task<Result<List<ProjectMember>>> GetMembersByProjectIdAsync(string projectId);

        Task<Result<List<ProjectMember>>> GetMembersByRoleIdAsync(string roleId);

        Task<Result<ProjectRole>> GetRoleByUserIdAsync(string userId);

        Task<Result<ProjectMember>> GetProjectMemberByIdAsync(string id);

        Task<Result<List<Project>>> GetProjectsByUserIdAsync(string userId);

        Task<Result<bool>> IsUserAlreadyMemberAsync(string userId, string projectId);
    }
}

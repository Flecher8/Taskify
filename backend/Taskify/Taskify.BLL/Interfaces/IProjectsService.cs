using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectsService
    {
        Task<Result<Project>> GetProjectByIdAsync(string id);
        Task<Result<Project>> CreateProjectAsync(Project project);
        Task<Result<bool>> UpdateProjectAsync(Project project);
        Task<Result<bool>> DeleteProjectAsync(string id);
        Task<Result<List<Project>>> GetProjectsByUserIdAsync(string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ITaskStatisticsService
    {
        Task<Result<List<SectionTypeTaskCountStatistics>>> GetSectionTypeTaskCountForProjectStatisticsAsync(string projectId);
        Task<Result<List<SectionTaskCountStatistics>>> GetTaskCountForSectionsAsync(string projectId);
        Task<Result<List<UserTaskCountStatistics>>> GetUserTaskCountForProjectStatisticsAsync(string projectId);
        Task<Result<List<UserStoryPointsCountStatistics>>> GetUserStoryPointsCountForProjectStatisticsAsync(string projectId);
        Task<Result<List<ProjectRoleTaskCountStatistics>>> GetTaskCountByRolesAsync(string projectId);
    }
}

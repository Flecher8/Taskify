using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;
using Taskify.Core.Result;

namespace Taskify.BLL.Services
{
    public class KpiStatisticsService : IKpiStatisticsService
    {
        private readonly ICustomTasksService _customTasksService;
        private readonly IProjectMembersService _projectMembersService;
        private readonly IProjectsService _projectsService;
        private readonly ILogger<KpiStatisticsService> _logger;

        public KpiStatisticsService(
            ICustomTasksService customTasksService,
            IProjectMembersService projectMembersService,
            IProjectsService projectsService,
            ILogger<KpiStatisticsService> logger
            )
        {
            _customTasksService = customTasksService;
            _projectMembersService = projectMembersService;
            _projectsService = projectsService;
            _logger = logger;
        }

        public async Task<Result<KpiStatistics>> GetKpiForUserAsync(string userId, string projectId)
        {
            try
            {
                var tasksResult = await _customTasksService.GetCustomTasksAssignedForUserInProjectAsync(projectId, userId);
                if (!tasksResult.IsSuccess)
                {
                    return ResultFactory.Failure<KpiStatistics>(tasksResult.Errors);
                }

                var tasks = tasksResult.Data;

                if (tasks.Count == 0)
                {
                    return ResultFactory.Failure<KpiStatistics>("No tasks assigned to the user.");
                }

                if (tasks.FirstOrDefault()?.ResponsibleUser == null)
                {
                    return ResultFactory.Failure<KpiStatistics>("Cannot find user with such id.");
                }

                var totalTasks = tasks.Count;
                var completedTasks = tasks.Count(t => t.Section.SectionType == SectionType.Done);
                var totalEstimatedTime = tasks.Sum(t => t.StoryPoints ?? 0);
                var totalTimeSpent = tasks.Sum(t => t.TaskTimeTrackers.Sum(tt => tt.DurationInSeconds)) / 3600.0; // Convert seconds to hours

                if (totalTasks == 0 || totalEstimatedTime == 0)
                {
                    return ResultFactory.Success(new KpiStatistics
                    {
                        User = tasks.FirstOrDefault().ResponsibleUser,
                        Kpi = 0
                    });
                }

                // Calculate scaled metrics
                var completionRateScaled = ((double)completedTasks / totalTasks) * 100;
                var timeEfficiencyScaled = Math.Min((totalEstimatedTime / totalTimeSpent) * 100, 100);

                // Weights
                const double w1 = 0.5;
                const double w2 = 0.5;

                var kpi = w1 * completionRateScaled + w2 * timeEfficiencyScaled;

                var kpiStatistics = new KpiStatistics
                {
                    User = tasks.FirstOrDefault().ResponsibleUser,
                    Kpi = kpi
                };

                return ResultFactory.Success(kpiStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<KpiStatistics>("An error occurred while retrieving KPI for the user.");
            }
        }


        public async Task<Result<List<KpiStatistics>>> GetKpiForAllUsersInProjectAsync(string projectId)
        {
            try
            {
                var projectResult = await _projectsService.GetProjectByIdAsync(projectId);
                if (!projectResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<KpiStatistics>>(projectResult.Errors);
                }

                var project = projectResult.Data;
                var projectOwner = project.User;

                var projectMembersResult = await _projectMembersService.GetMembersByProjectIdAsync(projectId);
                if (!projectMembersResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<KpiStatistics>>(projectMembersResult.Errors);
                }

                var projectMembers = projectMembersResult.Data;
                var users = projectMembers.Select(pm => pm.User).ToList();

                if (projectOwner != null)
                {
                    users.Add(projectOwner);
                }

                var kpiStatisticsList = new List<KpiStatistics>();
                foreach (var user in users)
                {
                    var kpiResult = await GetKpiForUserAsync(user.Id, projectId);
                    if (kpiResult.IsSuccess)
                    {
                        kpiStatisticsList.Add(kpiResult.Data);
                    }
                }

                return ResultFactory.Success(kpiStatisticsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<KpiStatistics>>("An error occurred while retrieving KPIs for all users in the project.");
            }
        }
    }
}

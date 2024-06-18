using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos.Statistics;
using Taskify.Core.Enums;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class TaskStatisticsService : ITaskStatisticsService
    {
        private readonly ISectionsService _sectionsService;
        private readonly ICustomTasksService _customTasksService;
        private readonly IProjectMembersService _projectMembersService;
        private readonly IProjectRolesService _projectRolesService;
        private readonly ILogger<TaskStatisticsService> _logger;
        private const int defaultStoryPoints = 1;

        public TaskStatisticsService(
            ISectionsService sectionService,
            ICustomTasksService customTasksService, 
            IProjectMembersService projectMembersService,
            IProjectRolesService projectRolesService,
            ILogger<TaskStatisticsService> logger
            )
        {
            _sectionsService = sectionService;
            _customTasksService = customTasksService;
            _projectMembersService = projectMembersService;
            _projectRolesService = projectRolesService;
            _logger = logger;
        }

        public async Task<Result<List<SectionTypeTaskCountStatistics>>> GetSectionTypeTaskCountForProjectStatisticsAsync(string projectId)
        {
            try
            {
                // Retrieve all sections in the project
                var sectionsResult = await _sectionsService.GetSectionsByProjectIdAsync(projectId);
                if (!sectionsResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<SectionTypeTaskCountStatistics>>(sectionsResult.Errors);
                }

                var sections = sectionsResult.Data;

                // Initialize the dictionary to store counts for each section type
                var sectionTypeCounts = new Dictionary<SectionType, int>();

                // Initialize the counts for each section type to 0
                foreach (SectionType sectionType in Enum.GetValues(typeof(SectionType)))
                {
                    sectionTypeCounts[sectionType] = 0;
                }

                // Loop through each section to count tasks by section type
                foreach (var section in sections)
                {
                    // Increment the count for the section type
                    sectionTypeCounts[section.SectionType] += section.CustomTasks.Count;
                }

                // Create the DTO to return the counts
                var sectionTypeTaskCountDto = new List<SectionTypeTaskCountStatistics>();

                // Transfer data to DTO
                foreach (SectionType sectionType in Enum.GetValues(typeof(SectionType)))
                {
                    var sectionTypeCount = new SectionTypeTaskCountStatistics();
                    sectionTypeCount.SectionType = sectionType;
                    sectionTypeCount.Count = sectionTypeCounts[sectionType];
                    sectionTypeTaskCountDto.Add(sectionTypeCount);
                }

                return ResultFactory.Success(sectionTypeTaskCountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<SectionTypeTaskCountStatistics>>("An error occurred while retrieving section type task counts for the project.");
            }
        }

        public async Task<Result<List<SectionTaskCountStatistics>>> GetTaskCountForSectionsAsync(string projectId)
        {
            try
            {
                // Retrieve all sections in the project
                var sectionsResult = await _sectionsService.GetSectionsByProjectIdAsync(projectId);
                if (!sectionsResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<SectionTaskCountStatistics>>(sectionsResult.Errors);
                }

                var sections = sectionsResult.Data;

                // Initialize a list to store task counts in each section
                var sectionTaskCount = new List<SectionTaskCountStatistics>();

                // Loop through each section to count tasks
                foreach (var section in sections)
                {
                    var taskCountDto = new SectionTaskCountStatistics
                    {
                        Section = section,
                        Count = section.CustomTasks.Count
                    };

                    sectionTaskCount.Add(taskCountDto);
                }

                return ResultFactory.Success(sectionTaskCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<SectionTaskCountStatistics>>("An error occurred while retrieving task counts for sections in the project.");
            }
        }

        public async Task<Result<List<UserStoryPointsCountStatistics>>> GetUserStoryPointsCountForProjectStatisticsAsync(string projectId)
        {
            try
            {
                // Find all tasks in the project
                var tasksResult = await _customTasksService.GetCustomTasksByProjectIdAsync(projectId);
                if (!tasksResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<UserStoryPointsCountStatistics>>(tasksResult.Errors);
                }

                var tasks = tasksResult.Data;

                // Initialize a dictionary to store task counts for each user
                var userTaskCounts = new Dictionary<User, int>();

                var placeholderUser = new User { Id = "placeholder" };

                // Loop through each task to count tasks for each user
                foreach (var task in tasks)
                {
                    int storyPoints = (int)(task.StoryPoints == null ? defaultStoryPoints : task.StoryPoints);
                    if (task.ResponsibleUser != null)
                    {
                        // Increment the count for the user
                        if (userTaskCounts.ContainsKey(task.ResponsibleUser))
                        {
                            userTaskCounts[task.ResponsibleUser] += storyPoints;
                        }
                        else
                        {
                            userTaskCounts[task.ResponsibleUser] = storyPoints;
                        }
                    }
                    else
                    {
                        // Increment the count for null user
                        if (userTaskCounts.ContainsKey(placeholderUser))
                        {
                            userTaskCounts[placeholderUser]++;
                        }
                        else
                        {
                            userTaskCounts[placeholderUser] = 1;
                        }
                    }
                }

                // Create a list to store user task counts statistics
                var userStoryPointsCountStatistics = new List<UserStoryPointsCountStatistics>();

                // Convert the dictionary to a list of UserStoryPointsCountStatistics objects
                foreach (var kvp in userTaskCounts)
                {
                    var userStoryPointsCountStat = new UserStoryPointsCountStatistics
                    {
                        User = kvp.Key.Id != placeholderUser.Id ? kvp.Key : null,
                        Count = kvp.Value
                    };
                    userStoryPointsCountStatistics.Add(userStoryPointsCountStat);
                }

                return ResultFactory.Success(userStoryPointsCountStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<UserStoryPointsCountStatistics>>("An error occurred while retrieving story points counts for each user in the project.");
            }
        }

        public async Task<Result<List<UserTaskCountStatistics>>> GetUserTaskCountForProjectStatisticsAsync(string projectId)
        {
            try
            {
                // Find all tasks in the project
                var tasksResult = await _customTasksService.GetCustomTasksByProjectIdAsync(projectId);
                if (!tasksResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<UserTaskCountStatistics>>(tasksResult.Errors);
                }

                var tasks = tasksResult.Data;

                // Initialize a dictionary to store task counts for each user
                var userTaskCounts = new Dictionary<User, int>();

                var placeholderUser = new User { Id = "placeholder" };

                // Loop through each task to count tasks for each user
                foreach (var task in tasks)
                {
                    if (task.ResponsibleUser != null)
                    {
                        // Increment the count for the user
                        if (userTaskCounts.ContainsKey(task.ResponsibleUser))
                        {
                            userTaskCounts[task.ResponsibleUser]++;
                        }
                        else
                        {
                            userTaskCounts[task.ResponsibleUser] = 1;
                        }
                    }
                    else
                    {
                        
                        if (userTaskCounts.ContainsKey(placeholderUser))
                        {
                            userTaskCounts[placeholderUser]++;
                        }
                        else
                        {
                            userTaskCounts[placeholderUser] = 1;
                        }
                    }
                }

                // Create a list to store user task counts statistics
                var userTaskCountStatistics = new List<UserTaskCountStatistics>();

                // Convert the dictionary to a list of UserTaskCountStatistics objects
                foreach (var kvp in userTaskCounts)
                {
                    var userTaskCountStat = new UserTaskCountStatistics
                    {
                        User = kvp.Key.Id != placeholderUser.Id ? kvp.Key : null,
                        Count = kvp.Value
                    };
                    userTaskCountStatistics.Add(userTaskCountStat);
                }

                return ResultFactory.Success(userTaskCountStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<UserTaskCountStatistics>>("An error occurred while retrieving task counts for each user in the project.");
            }
        }


        public async Task<Result<List<ProjectRoleTaskCountStatistics>>> GetTaskCountByRolesAsync(string projectId)
        {
            try
            {
                // Get all project members
                var projectMembersResult = await _projectMembersService.GetMembersByProjectIdAsync(projectId);
                if (!projectMembersResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectRoleTaskCountStatistics>>(projectMembersResult.Errors);
                }

                // Get all project roles
                var projectRolesResult = await _projectRolesService.GetRolesByProjectIdAsync(projectId);
                if (!projectRolesResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectRoleTaskCountStatistics>>(projectRolesResult.Errors);
                }

                var projectMembers = projectMembersResult.Data;
                var projectRoles = projectRolesResult.Data;

                // Initialize a dictionary to store task counts for each role
                var roleTaskCounts = new Dictionary<ProjectRole, int>();

                var placeholderRole = new ProjectRole { Id = "placeholder" };

                // Loop through each project member to count tasks for each role
                foreach (var member in projectMembers)
                {
                    var role = member.ProjectRole;
                    if (role == null)
                    {
                        role = placeholderRole;
                    }

                    if (!roleTaskCounts.ContainsKey(role))
                    {
                        roleTaskCounts[role] = 0;
                    }

                    // Count tasks assigned to the member (Errors)
                    var tasksResult = await _customTasksService.GetCustomTasksAssignedForUserInProjectAsync(projectId, member.User.Id);
                    if (tasksResult.IsSuccess)
                    {
                        roleTaskCounts[role] += tasksResult.Data.Count;
                    }
                }

                // Count total tasks in the project
                var allTasksResult = await _customTasksService.GetCustomTasksByProjectIdAsync(projectId);
                if (!allTasksResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectRoleTaskCountStatistics>>(allTasksResult.Errors);
                }

                var allTasks = allTasksResult.Data;
                var totalTaskCount = allTasks.Count;

                // Calculate tasks not assigned to any roles
                var tasksAssignedToRoles = roleTaskCounts.Values.Sum();
                var tasksNotAssignedToRoles = totalTaskCount - tasksAssignedToRoles;

                if (roleTaskCounts.ContainsKey(placeholderRole))
                {
                    roleTaskCounts[placeholderRole] += tasksNotAssignedToRoles;
                }
                else
                {
                    roleTaskCounts[placeholderRole] = tasksNotAssignedToRoles;
                }
                

                // Create a list to store role task count statistics
                var roleTaskCountStatistics = new List<ProjectRoleTaskCountStatistics>();

                // Convert the dictionary to a list of ProjectRoleTaskCountStatistics objects
                foreach (var kvp in roleTaskCounts)
                {
                    var roleTaskCountStat = new ProjectRoleTaskCountStatistics
                    {
                        ProjectRole = kvp.Key.Id != placeholderRole.Id ? kvp.Key : null,
                        Count = kvp.Value
                    };
                    roleTaskCountStatistics.Add(roleTaskCountStat);
                }

                return ResultFactory.Success(roleTaskCountStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectRoleTaskCountStatistics>>("An error occurred while retrieving task counts for each role in the project.");
            }
        }

    }
}

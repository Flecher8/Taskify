using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Repositories;

namespace Taskify.BLL.Services
{
    public class TaskTimeTrackersService : ITaskTimeTrackersService
    {
        private readonly ILogger<TaskTimeTrackersService> _logger;
        private readonly IValidator<TaskTimeTracker> _validator;
        private readonly IUserRepository _userRepository;
        private readonly ICustomTaskRepository _customTaskRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly ITaskTimeTrackerRepository _taskTimeTrackerRepository;

        public TaskTimeTrackersService(IProjectRepository projectRepository,
            IValidator<TaskTimeTracker> validator,
            ILogger<TaskTimeTrackersService> logger,
            IUserRepository userRepository,
            ICustomTaskRepository customTaskRepository,
            ISectionRepository sectionRepository,
            ITaskTimeTrackerRepository taskTimeTrackerRepository
        )
        {
            _validator = validator;
            _logger = logger;
            _userRepository = userRepository;
            _customTaskRepository = customTaskRepository;
            _sectionRepository = sectionRepository;
            _taskTimeTrackerRepository = taskTimeTrackerRepository;
        }

        public async Task<Result<TaskTimeTracker>> CreateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker)
        {
            try
            {
                if (taskTimeTracker.User == null)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Can not find user.");
                }

                if (taskTimeTracker.CustomTask == null)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Can not find task.");
                }

                var userResult = await _userRepository.GetByIdAsync(taskTimeTracker.User.Id);

                if (userResult == null)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Can not find user by this id.");
                }

                var taskResult = await _customTaskRepository.GetByIdAsync(taskTimeTracker.CustomTask.Id);

                if (taskResult == null)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Can not find task by this id.");
                }

                // Validation
                var validation = await _validator.ValidateAsync(taskTimeTracker);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<TaskTimeTracker>(validation.ErrorMessages);
                }

                taskTimeTracker.CreatedAt = DateTime.UtcNow;
                taskTimeTracker.User = userResult;
                taskTimeTracker.CustomTask = taskResult;

                if(taskTimeTracker.TrackerType == TaskTimeTrackerType.Stopwatch)
                {
                    taskTimeTracker.EndDateTime = null;
                    taskTimeTracker.DurationInSeconds = 0;
                }

                if (taskTimeTracker.TrackerType == TaskTimeTrackerType.Range)
                {
                    if (taskTimeTracker.EndDateTime != null)
                    {
                        TimeSpan difference = (TimeSpan)(taskTimeTracker.EndDateTime - taskTimeTracker.StartDateTime);
                        double differenceInSeconds = difference.TotalSeconds;
                        taskTimeTracker.DurationInSeconds = (uint)differenceInSeconds;
                    }
                    else
                    {
                        taskTimeTracker.DurationInSeconds = 0;
                    }
                }

                

                var result = await _taskTimeTrackerRepository.AddAsync(taskTimeTracker);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<TaskTimeTracker>("Can not create a new task time tracker.");
            }
        }

        public async Task<Result<bool>> DeleteTaskTimeTrackerAsync(string id)
        {
            try
            {
                await _taskTimeTrackerRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not create a new task time tracker.");
            }
        }

        public async Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByUserForTaskAsync(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(tracker => tracker.User.Id == userId && tracker.CustomTask.Id == taskId)
                    );

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<TaskTimeTracker>>("Can not get task time trackers by user and task.");
            }
        }

        public async Task<Result<TaskTimeTracker>> StartTimerAsync(string userId, string taskId)
        {
            try
            {
                var listOfStartedTimers = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(
                                    tracker => tracker.User.Id == userId && 
                                    tracker.CustomTask.Id == taskId && 
                                    tracker.TrackerType == TaskTimeTrackerType.Stopwatch && 
                                    tracker.EndDateTime == null
                                )
                    );

                if (listOfStartedTimers.Count != 0)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Can not start new timer, stop old one.");
                }

                return ResultFactory.Success(await CreateTaskTimeTrackerAsync(CreateNewTimer(userId, taskId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<TaskTimeTracker>("Can not create a new timer.");
            }
        }

        private TaskTimeTracker CreateNewTimer(string userId, string taskId)
        {
            TaskTimeTracker taskTimeTracker = new TaskTimeTracker();
            taskTimeTracker.User = new User();
            taskTimeTracker.User.Id = userId;
            taskTimeTracker.CustomTask = new CustomTask();
            taskTimeTracker.CustomTask.Id = taskId;
            taskTimeTracker.StartDateTime = DateTime.UtcNow;
            taskTimeTracker.EndDateTime = null;
            taskTimeTracker.CreatedAt = DateTime.UtcNow;
            taskTimeTracker.DurationInSeconds = 0;
            taskTimeTracker.TrackerType = TaskTimeTrackerType.Stopwatch;
            return taskTimeTracker;
        }

        public async Task<Result<bool>> StopTimerAsync(string userId, string taskId)
        {
            try
            {
                var timerToStop = (await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(
                                    tracker => tracker.User.Id == userId &&
                                    tracker.CustomTask.Id == taskId &&
                                    tracker.TrackerType == TaskTimeTrackerType.Stopwatch &&
                                    tracker.EndDateTime == null
                                )
                    )).FirstOrDefault();

                if (timerToStop == null)
                {
                    return ResultFactory.Failure<bool>("Can not find started timer, create new one.");
                }

                var timerToUpdate = timerToStop;

                timerToUpdate.EndDateTime = DateTime.UtcNow;

                return ResultFactory.Success(await UpdateTaskTimeTrackerAsync(timerToUpdate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not create a new timer.");
            }
        }

        public async Task<Result<bool>> UpdateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(taskTimeTracker);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var taskTimeTrackerToUpdate = (await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(tracker => tracker.Id == taskTimeTracker.Id)
                    )).FirstOrDefault();

                if (taskTimeTrackerToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find task time tracker with such id.");
                }

                taskTimeTrackerToUpdate.StartDateTime = taskTimeTracker.StartDateTime;
                taskTimeTrackerToUpdate.EndDateTime = taskTimeTracker.EndDateTime;
                taskTimeTrackerToUpdate.Description = taskTimeTracker.Description;

                if (taskTimeTracker.EndDateTime != null)
                {
                    TimeSpan difference = (TimeSpan)(taskTimeTracker.EndDateTime - taskTimeTracker.StartDateTime);
                    double differenceInSeconds = difference.TotalSeconds;
                    taskTimeTracker.DurationInSeconds = (uint)differenceInSeconds;
                }
                else
                {
                    taskTimeTracker.DurationInSeconds = 0;
                }

                await _taskTimeTrackerRepository.UpdateAsync(taskTimeTracker);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not create a new task time tracker.");
            }
        }

        public async Task<Result<TaskTimeTracker>> GetTaskTimeTrackerByIdAsync(string id)
        {
            try
            {
                var result = (await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                    builder => builder
                    .IncludeUserEntity()
                    .IncludeCustomTaskEntity()
                    .WithFilter(p => p.Id == id)
                )).FirstOrDefault();

                if (result == null)
                {
                    return ResultFactory.Failure<TaskTimeTracker>("Task Time Tracker with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<TaskTimeTracker>("Can not get the Task Time Tracker by id.");
            }
        }

        public async Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByTaskAsync(string taskId)
        {
            try
            {
                var taskTimeTrackersByTask = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(tracker => tracker.CustomTask.Id == taskId)
                    );

                return ResultFactory.Success(taskTimeTrackersByTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<TaskTimeTracker>>("Can not get task time trackers by task.");
            }
        }

        public async Task<Result<ulong>> GetNumberOfSecondsSpendOnTask(string taskId)
        {
            try
            {
                var taskTimeTrackersByTask = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeCustomTaskEntity()
                            .WithFilter(tracker => tracker.CustomTask.Id == taskId)
                    );

                ulong totalSecondsSpent = 0;

                foreach (var tracker in taskTimeTrackersByTask)
                {
                    if (tracker.EndDateTime != null)
                    {
                        TimeSpan duration = (TimeSpan)(tracker.EndDateTime - tracker.StartDateTime);
                        totalSecondsSpent += (ulong)duration.TotalSeconds;
                    }
                }

                return ResultFactory.Success(totalSecondsSpent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ulong>("Can not get number of seconds spend on task.");
            }
        }

        public async Task<Result<TaskTimeTracker?>> IsTimerActiveAsync(string userId, string taskId)
        {
            try
            {
                var activeTimer = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .IncludeCustomTaskEntity()
                            .WithFilter(
                                tracker => tracker.User.Id == userId &&
                                tracker.CustomTask.Id == taskId &&
                                tracker.TrackerType == TaskTimeTrackerType.Stopwatch &&
                                tracker.EndDateTime == null
                            )
                    );

                return ResultFactory.Success(activeTimer.FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<TaskTimeTracker?>("An error occurred while checking for active timer.");
            }
        }

        public async Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByUserAndDateInProjectAsync
            (string projectId, 
             string userId,
             DateTime date
            )
        {
            try
            {
                var sections = await _sectionRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .WithFilter(s => s.Project.Id == projectId)
                );

                var tasks = new List<CustomTask>();
                // Asynchronously retrieve custom tasks for each section
                var sectionTasks = sections.Select(section => GetCustomTasksBySectionIdAsync(section.Id));

                // Wait for all tasks to complete
                await Task.WhenAll(sectionTasks);

                // Add custom tasks from each section to the result list
                foreach (var sectionTask in sectionTasks)
                {
                    var customTaskResult = await sectionTask;
                    if (!customTaskResult.IsSuccess)
                    {
                        return ResultFactory.Failure<List<TaskTimeTracker>>(customTaskResult.Errors);
                    }
                    tasks.AddRange(customTaskResult.Data);
                }

                // Get task time trackers by user ID and custom task IDs
                var taskTimeTrackers = await _taskTimeTrackerRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCustomTaskEntity()
                        .WithFilter(tracker =>
                            tracker.User.Id == userId &&
                            tasks.Select(t => t.Id).Contains(tracker.CustomTask.Id) &&
                            // StartDateTime should be less than or equal to the given date
                            tracker.StartDateTime.Date <= date.Date &&
                            // Either EndDateTime is null or it should be greater than or equal to the given date
                            (tracker.EndDateTime == null || date.Date <= tracker.EndDateTime.Value.Date) 
                           
                        )
                );


                return ResultFactory.Success(taskTimeTrackers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<TaskTimeTracker>>("Can not get task time trackers by user and date in project.");
            }
        }

        private async Task<Result<List<CustomTask>>> GetCustomTasksBySectionIdAsync(string sectionId)
        {
            try
            {
                var result = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .IncludeResponsibleUserEntity()
                        .IncludeTaskTimeTrackersEntity()
                        .WithFilter(c => c.Section != null && c.Section.Id == sectionId)
                );

                result = result.OrderBy(c => c.SequenceNumber).ToList();

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CustomTask>>("Can not get custom tasks by section id.");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly ITaskTimeTrackerRepository _taskTimeTrackerRepository;

        public TaskTimeTrackersService(IProjectRepository projectRepository,
            IValidator<TaskTimeTracker> validator,
            ILogger<TaskTimeTrackersService> logger,
            IUserRepository userRepository,
            ICustomTaskRepository customTaskRepository,
            ITaskTimeTrackerRepository taskTimeTrackerRepository
        )
        {
            _validator = validator;
            _logger = logger;
            _userRepository = userRepository;
            _customTaskRepository = customTaskRepository;
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

                if(taskTimeTracker.TrackerType == TaskTimeTrackerType.Timer)
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

        public async Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackerByUserForTaskAsync(string userId, string taskId)
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
                return ResultFactory.Failure<List<TaskTimeTracker>>("Can not create a new task time tracker.");
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
                                    tracker.TrackerType == TaskTimeTrackerType.Timer && 
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
            taskTimeTracker.User.Id = userId;
            taskTimeTracker.CustomTask.Id = taskId;
            taskTimeTracker.StartDateTime = DateTime.UtcNow;
            taskTimeTracker.EndDateTime = null;
            taskTimeTracker.CreatedAt = DateTime.UtcNow;
            taskTimeTracker.DurationInSeconds = 0;
            taskTimeTracker.TrackerType = TaskTimeTrackerType.Timer;
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
                                    tracker.TrackerType == TaskTimeTrackerType.Timer &&
                                    tracker.EndDateTime == null
                                )
                    )).FirstOrDefault();

                if (timerToStop == null)
                {
                    return ResultFactory.Failure<bool>("Can not find started timer, create new one.");
                }

                timerToStop.EndDateTime = DateTime.UtcNow;

                return ResultFactory.Success(await UpdateTaskTimeTrackerAsync(timerToStop));
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

                var result = await _taskTimeTrackerRepository.AddAsync(taskTimeTracker);

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
    }
}

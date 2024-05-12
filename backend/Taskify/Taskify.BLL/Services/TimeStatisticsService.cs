using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Services
{
    public class TimeStatisticsService : ITimeStatisticsService
    {
        private readonly TaskTimeTrackersService _taskTimeTrackersService;
        private readonly ILogger<TimeStatisticsService> _logger;

        public TimeStatisticsService(TaskTimeTrackersService taskTimeTrackersService, ILogger<TimeStatisticsService> logger)
        {
            _taskTimeTrackersService = taskTimeTrackersService;
            _logger = logger;
        }

        public async Task<Result<UserTimeSpendOnDateStatistic>> GetUserProjectTimeStatisticsAsync(UserTimeSpendOnDateRequest request)
        {
            try
            {
                // Get all task time trackers for the user, project, and date
                var taskTimeTrackersResult = await _taskTimeTrackersService.GetTaskTimeTrackersByUserAndDateInProjectAsync(
                    request.ProjectId, request.UserId, request.Date);

                if (!taskTimeTrackersResult.IsSuccess)
                {
                    return ResultFactory.Failure<UserTimeSpendOnDateStatistic>(taskTimeTrackersResult.Errors);
                }

                var taskTimeTrackers = taskTimeTrackersResult.Data;

                // Calculate the total number of seconds spent on the specified date
                ulong totalSeconds = CalculateTotalSecondsOnDate(taskTimeTrackers, request.Date);

                return ResultFactory.Success(new UserTimeSpendOnDateStatistic { SecondsSpend = totalSeconds });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<UserTimeSpendOnDateStatistic>($"An error occurred: {ex.Message}");
            }
        }

        private ulong CalculateTotalSecondsOnDate(List<TaskTimeTracker> taskTimeTrackers, DateTime date)
        {
            ulong totalSeconds = 0;

            foreach (var tracker in taskTimeTrackers)
            {
                DateTime startTime = tracker.StartDateTime;
                DateTime endTime = tracker.EndDateTime ?? DateTime.UtcNow;

                // Ensure the startDateTime is not after the specified date
                if (startTime.Date > date.Date)
                    continue; // This tracker doesn't contribute to the total

                // Ensure the endDateTime is not before the specified date
                if (endTime.Date < date.Date)
                    continue; // This tracker doesn't contribute to the total        

                // Calculate the time interval within the specified date
                DateTime intervalStart = startTime.Date == date.Date ? startTime : date.Date;
                DateTime intervalEnd = endTime.Date > date.Date ? date.Date.AddDays(1) : endTime;

                // Calculate the total seconds in the interval
                totalSeconds += (ulong)(intervalEnd - intervalStart).TotalSeconds;
            }

            return totalSeconds;
        }
    }
}

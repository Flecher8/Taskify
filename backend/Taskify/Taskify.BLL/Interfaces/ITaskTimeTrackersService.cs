using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ITaskTimeTrackersService
    {
        Task<Result<TaskTimeTracker>> CreateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker);
        Task<Result<bool>> UpdateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker);
        Task<Result<bool>> DeleteTaskTimeTrackerAsync(string id);
        Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByUserForTaskAsync(string userId, string taskId);
        Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByTaskAsync(string taskId);
        Task<Result<TaskTimeTracker>> StartTimerAsync(string userId, string taskId);
        Task<Result<bool>> StopTimerAsync(string userId, string taskId);
        Task<Result<TaskTimeTracker>> GetTaskTimeTrackerByIdAsync(string id);
        Task<Result<ulong>> GetNumberOfSecondsSpendOnTask(string taskId);
        Task<Result<TaskTimeTracker?>> IsTimerActiveAsync(string userId, string taskId);
        Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackersByUserAndDateInProjectAsync(string projectId, string userId, DateTime date);
    }
}

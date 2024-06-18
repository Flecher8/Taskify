using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICustomTasksService
    {
        Task<Result<CustomTask>> CreateCustomTaskAsync(CustomTask customTask);
        Task<Result<bool>> DeleteCustomTaskAsync(string id);
        Task<Result<CustomTask>> GetCustomTaskByIdAsync(string id);
        Task<Result<List<CustomTask>>> GetCustomTasksBySectionIdAsync(string sectionId);
        Task<Result<List<CustomTask>>> GetCustomTasksByProjectIdAsync(string projectId);
        Task<Result<bool>> UpdateCustomTaskAsync(CustomTask customTask);
        Task<Result<bool>> MoveCustomTaskAsync(string customTaskId, int targetSequenceNumber);
        Task<Result<bool>> ArchiveCustomTaskAsync(string customTaskId);
        Task<Result<bool>> UnarchiveCustomTaskAsync(string customTaskId);
        Task<Result<List<CustomTask>>> GetArchivedCustomTasksByProjectAsync(string projectId);
        Task<Result<bool>> RedirectCustomTaskAsync(string customTaskId, string targetSectionId, int? targetSequenceNumber);
        Task<Result<List<CustomTask>>> GetCustomTasksAssignedForUserInProjectAsync(string projectId, string userId);
    }
}

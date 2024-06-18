using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IKpiStatisticsService
    {
        Task<Result<KpiStatistics>> GetKpiForUserAsync(string userId, string projectId);
        Task<Result<List<KpiStatistics>>> GetKpiForAllUsersInProjectAsync(string projectId);
    }
}

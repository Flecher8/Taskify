using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectIncomesService
    {
        Task<Result<ProjectIncome>> CreateProjectIncomeAsync(ProjectIncome projectIncome);
        Task<Result<bool>> DeleteProjectIncomeAsync(string id);
        Task<Result<List<ProjectIncome>>> GetProjectIncomesByProjectIdAsync(string projectId);
        Task<Result<bool>> UpdateProjectIncomeAsync(ProjectIncome projectIncome);
    }
}

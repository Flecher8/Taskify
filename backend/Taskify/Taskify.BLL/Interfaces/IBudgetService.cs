using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IBudgetService
    {
        Task<Result<FinancialStatistics>> GetMonthlyIncomeStatisticsAsync(string companyId);
        Task<Result<FinancialStatistics>> GetMonthlyExpenseStatisticsAsync(string companyId);
        Task<Result<FinancialStatistics>> GetYearlyIncomeStatisticsAsync(string companyId);
        Task<Result<FinancialStatistics>> GetYearlyExpenseStatisticsAsync(string companyId);
        Task<Result<List<ProjectIncomeDistributionStatistics>>> GetMonthlyIncomeDistributionByProjectsAsync(string companyId);
        Task<Result<List<ProjectIncomeDistributionStatistics>>> GetYearlyIncomeDistributionByProjectsAsync(string companyId);
        Task<Result<List<RoleSalaryStatistics>>> GetTotalSalariesByRoleAsync(string companyId);
    }
}

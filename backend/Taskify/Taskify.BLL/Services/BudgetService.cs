using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Taskify.BLL.Helpers;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ILogger<BudgetService> _logger;
        private readonly ICompaniesService _companiesService;
        private readonly ICompanyMembersService _companyMembersService;
        private readonly ICompanyExpensesService _companyExpensesService;
        private readonly IProjectIncomesService _projectIncomesService;

        public BudgetService(
            ILogger<BudgetService> logger,
            ICompaniesService companiesService,
            ICompanyMembersService companyMembersService,
            ICompanyExpensesService companyExpensesService,
            IProjectIncomesService projectIncomesService)
        {
            _logger = logger;
            _companiesService = companiesService;
            _companyMembersService = companyMembersService;
            _companyExpensesService = companyExpensesService;
            _projectIncomesService = projectIncomesService;
        }

        public async Task<Result<FinancialStatistics>> GetMonthlyIncomeStatisticsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(companyResult.Errors);
                }

                var incomesResult = await _projectIncomesService.GetProjectIncomesByUserIdAsync(companyResult.Data.User.Id);

                if (!incomesResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(incomesResult.Errors);
                }

                var monthlyIncome = incomesResult.Data
                    .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Monthly)
                    .Sum(pi => pi.Amount);

                var statistic = new FinancialStatistics
                {
                    Name = "Monthly Income",
                    Value = monthlyIncome
                };
                return ResultFactory.Success(statistic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<FinancialStatistics>("Failed to get monthly income statistics.");
            }
        }

        public async Task<Result<FinancialStatistics>> GetMonthlyExpenseStatisticsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(companyResult.Errors);
                }

                var membersResult = await _companyMembersService.GetMembersByCompanyIdAsync(companyId);
                if (!membersResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(membersResult.Errors);
                }

                var expensesResult = await _companyExpensesService.GetCompanyExpensesByCompanyIdAsync(companyId);
                if (!expensesResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(expensesResult.Errors);
                }

                var monthlyExpenses = membersResult.Data.Sum(member => member.Salary) +
                                      expensesResult.Data.Where(expense => expense.Frequency == Core.Enums.CompanyExpenseFrequency.Monthly)
                                                         .Sum(expense => expense.Amount);

                var statistic = new FinancialStatistics
                {
                    Name = "Monthly Expense",
                    Value = monthlyExpenses
                };
                return ResultFactory.Success(statistic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<FinancialStatistics>("Failed to get monthly expense statistics.");
            }
        }

        public async Task<Result<FinancialStatistics>> GetYearlyIncomeStatisticsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(companyResult.Errors);
                }

                var incomesResult = await _projectIncomesService.GetProjectIncomesByUserIdAsync(companyResult.Data.User.Id);

                if (!incomesResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(incomesResult.Errors);
                }

                var yearlyIncome = incomesResult.Data
                    .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Yearly)
                    .Sum(pi => pi.Amount) +
                    (incomesResult.Data
                    .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Monthly)
                    .Sum(pi => pi.Amount) * 12);

                var statistic = new FinancialStatistics
                {
                    Name = "Yearly Income",
                    Value = yearlyIncome
                };
                return ResultFactory.Success(statistic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<FinancialStatistics>("Failed to get yearly income statistics.");
            }
        }

        public async Task<Result<FinancialStatistics>> GetYearlyExpenseStatisticsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(companyResult.Errors);
                }

                var membersResult = await _companyMembersService.GetMembersByCompanyIdAsync(companyId);
                if (!membersResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(membersResult.Errors);
                }

                var expensesResult = await _companyExpensesService.GetCompanyExpensesByCompanyIdAsync(companyId);
                if (!expensesResult.IsSuccess)
                {
                    return ResultFactory.Failure<FinancialStatistics>(expensesResult.Errors);
                }

                var yearlyExpenses = (membersResult.Data.Sum(member => member.Salary) * 12) +
                                     expensesResult.Data
                                         .Where(expense => expense.Frequency == Core.Enums.CompanyExpenseFrequency.Yearly)
                                         .Sum(expense => expense.Amount) +
                                     (expensesResult.Data
                                         .Where(expense => expense.Frequency == Core.Enums.CompanyExpenseFrequency.Monthly)
                                         .Sum(expense => expense.Amount) * 12);

                var statistic = new FinancialStatistics
                {
                    Name = "Yearly Expense",
                    Value = yearlyExpenses
                };
                return ResultFactory.Success(statistic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<FinancialStatistics>("Failed to get yearly expense statistics.");
            }
        }

        public async Task<Result<List<ProjectIncomeDistributionStatistics>>> GetMonthlyIncomeDistributionByProjectsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>(companyResult.Errors);
                }

                var incomesResult = await _projectIncomesService.GetProjectIncomesByUserIdAsync(companyResult.Data.User.Id);

                if (!incomesResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>(incomesResult.Errors);
                }

                var incomeDistribution = incomesResult.Data
                    .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Monthly)
                    .GroupBy(pi => pi.Project)
                    .Select(group => new ProjectIncomeDistributionStatistics
                    {
                        Project = group.Key,
                        TotalIncome = group.Sum(pi => pi.Amount)
                    })
                    .ToList();

                return ResultFactory.Success(incomeDistribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>("Failed to get monthly income distribution by projects.");
            }
        }

        public async Task<Result<List<ProjectIncomeDistributionStatistics>>> GetYearlyIncomeDistributionByProjectsAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>(companyResult.Errors);
                }

                var incomesResult = await _projectIncomesService.GetProjectIncomesByUserIdAsync(companyResult.Data.User.Id);

                if (!incomesResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>(incomesResult.Errors);
                }

                var incomeDistribution = incomesResult.Data
                    .GroupBy(pi => pi.Project)
                    .Select(group => new ProjectIncomeDistributionStatistics
                    {
                        Project = group.Key,
                        TotalIncome = group
                            .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Yearly)
                            .Sum(pi => pi.Amount) +
                            (group
                            .Where(pi => pi.Frequency == Core.Enums.ProjectIncomeFrequency.Monthly)
                            .Sum(pi => pi.Amount) * 12)
                    })
                    .ToList();

                return ResultFactory.Success(incomeDistribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectIncomeDistributionStatistics>>("Failed to get yearly income distribution by projects.");
            }
        }

        public async Task<Result<List<RoleSalaryStatistics>>> GetTotalSalariesByRoleAsync(string companyId)
        {
            try
            {
                var companyResult = await _companiesService.GetCompanyByIdAsync(companyId);
                if (!companyResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<RoleSalaryStatistics>>(companyResult.Errors);
                }

                var membersResult = await _companyMembersService.GetMembersByCompanyIdAsync(companyId);
                if (!membersResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<RoleSalaryStatistics>>(membersResult.Errors);
                }

                var roleSalaryStatistics = membersResult.Data
                    .GroupBy(member => member.Role)
                    .Select(group => new RoleSalaryStatistics
                    {
                        Role = group.Key,
                        TotalSalary = group.Sum(member => member.Salary)
                    })
                    .ToList();

                return ResultFactory.Success(roleSalaryStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<RoleSalaryStatistics>>("Failed to get total salaries by role.");
            }
        }
    }
}

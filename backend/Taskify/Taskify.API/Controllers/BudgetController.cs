using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.Dtos.Statistics;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;
        private readonly ILogger<BudgetController> _logger;

        public BudgetController(IBudgetService budgetService, IMapper mapper, ILogger<BudgetController> logger)
        {
            _budgetService = budgetService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("monthly-income/{companyId}")]
        public async Task<IActionResult> GetMonthlyIncomeStatistics(string companyId)
        {
            try
            {
                var result = await _budgetService.GetMonthlyIncomeStatisticsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<FinancialStatisticsDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMonthlyIncomeStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("monthly-expense/{companyId}")]
        public async Task<IActionResult> GetMonthlyExpenseStatistics(string companyId)
        {
            try
            {
                var result = await _budgetService.GetMonthlyExpenseStatisticsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<FinancialStatisticsDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMonthlyExpenseStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("yearly-income/{companyId}")]
        public async Task<IActionResult> GetYearlyIncomeStatistics(string companyId)
        {
            try
            {
                var result = await _budgetService.GetYearlyIncomeStatisticsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<FinancialStatisticsDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetYearlyIncomeStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("yearly-expense/{companyId}")]
        public async Task<IActionResult> GetYearlyExpenseStatistics(string companyId)
        {
            try
            {
                var result = await _budgetService.GetYearlyExpenseStatisticsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<FinancialStatisticsDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetYearlyExpenseStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("monthly-income-distribution/{companyId}")]
        public async Task<IActionResult> GetMonthlyIncomeDistributionByProjects(string companyId)
        {
            try
            {
                var result = await _budgetService.GetMonthlyIncomeDistributionByProjectsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectIncomeDistributionStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMonthlyIncomeDistributionByProjects method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("yearly-income-distribution/{companyId}")]
        public async Task<IActionResult> GetYearlyIncomeDistributionByProjects(string companyId)
        {
            try
            {
                var result = await _budgetService.GetYearlyIncomeDistributionByProjectsAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectIncomeDistributionStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetYearlyIncomeDistributionByProjects method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("total-salaries-by-role/{companyId}")]
        public async Task<IActionResult> GetTotalSalariesByRole(string companyId)
        {
            try
            {
                var result = await _budgetService.GetTotalSalariesByRoleAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<RoleSalaryStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTotalSalariesByRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

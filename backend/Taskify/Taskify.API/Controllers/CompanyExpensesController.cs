using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyExpensesController : ControllerBase
    {
        private readonly ICompanyExpensesService _companyExpensesService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyExpensesController> _logger;

        public CompanyExpensesController(ICompanyExpensesService companyExpensesService, IMapper mapper, ILogger<CompanyExpensesController> logger)
        {
            _companyExpensesService = companyExpensesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyExpenseById(string id)
        {
            try
            {
                var result = await _companyExpensesService.GetCompanyExpenseByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyExpenseDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyExpenseById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyExpense([FromBody] CreateCompanyExpenseDto companyExpenseDto)
        {
            try
            {
                var companyExpense = _mapper.Map<CompanyExpense>(companyExpenseDto);
                var result = await _companyExpensesService.CreateCompanyExpenseAsync(companyExpense);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyExpenseById), new { id = result.Data.Id }, _mapper.Map<CompanyExpenseDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompanyExpense method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyExpense(string id, [FromBody] UpdateCompanyExpenseDto companyExpenseDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != companyExpenseDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var companyExpense = _mapper.Map<CompanyExpense>(companyExpenseDto);
                var result = await _companyExpensesService.UpdateCompanyExpenseAsync(companyExpense);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCompanyExpense method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyExpense(string id)
        {
            try
            {
                var result = await _companyExpensesService.DeleteCompanyExpenseAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompanyExpense method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyExpensesByCompanyId(string companyId)
        {
            try
            {
                var result = await _companyExpensesService.GetCompanyExpensesByCompanyIdAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyExpenseDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyExpensesByCompanyId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _companiesService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompaniesService companiesService, IMapper mapper, ILogger<CompaniesController> logger)
        {
            _companiesService = companiesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(string id)
        {
            try
            {
                var result = await _companiesService.GetCompanyByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto companyDto)
        {
            try
            {
                var company = _mapper.Map<Company>(companyDto);
                var result = await _companiesService.CreateCompanyAsync(company);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyById), new { id = result.Data.Id }, _mapper.Map<CompanyDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompany method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyDto companyDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != companyDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var company = _mapper.Map<Company>(companyDto);
                var result = await _companiesService.UpdateCompanyAsync(company);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCompany method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                var result = await _companiesService.DeleteCompanyAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompany method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCompaniesByUserId(string userId)
        {
            try
            {
                var result = await _companiesService.GetCompaniesByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompaniesByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

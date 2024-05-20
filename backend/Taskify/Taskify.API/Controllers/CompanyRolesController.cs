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
    public class CompanyRolesController : ControllerBase
    {
        private readonly ICompanyRolesService _companyRolesService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyRolesController> _logger;

        public CompanyRolesController(ICompanyRolesService companyMemberRolesService, IMapper mapper, ILogger<CompanyRolesController> logger)
        {
            _companyRolesService = companyMemberRolesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyRoleById(string id)
        {
            try
            {
                var result = await _companyRolesService.GetCompanyRoleByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyRoleDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyRoleById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyRole([FromBody] CreateCompanyRoleDto companyRoleDto)
        {
            try
            {
                var companyMemberRole = _mapper.Map<CompanyRole>(companyRoleDto);
                var result = await _companyRolesService.CreateCompanyRoleAsync(companyMemberRole);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyRoleById), new { id = result.Data.Id }, _mapper.Map<CompanyRoleDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompanyRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyRole(string id, [FromBody] UpdateCompanyRoleDto companyRoleDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != companyRoleDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var companyMemberRole = _mapper.Map<CompanyRole>(companyRoleDto);
                var result = await _companyRolesService.UpdateCompanyRoleAsync(companyMemberRole);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCompanyRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyRole(string id)
        {
            try
            {
                var result = await _companyRolesService.DeleteCompanyRoleAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompanyRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyRolesByCompanyId(string companyId)
        {
            try
            {
                var result = await _companyRolesService.GetCompanyRolesByCompanyIdAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyRoleDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyRolesByCompanyId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

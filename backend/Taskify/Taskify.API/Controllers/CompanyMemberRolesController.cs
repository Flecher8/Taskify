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
    public class CompanyMemberRolesController : ControllerBase
    {
        private readonly ICompanyMemberRolesService _companyMemberRolesService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyMemberRolesController> _logger;

        public CompanyMemberRolesController(ICompanyMemberRolesService companyMemberRolesService, IMapper mapper, ILogger<CompanyMemberRolesController> logger)
        {
            _companyMemberRolesService = companyMemberRolesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyMemberRoleById(string id)
        {
            try
            {
                var result = await _companyMemberRolesService.GetCompanyMemberRoleByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyMemberRoleDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyMemberRoleById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyMemberRole([FromBody] CreateCompanyMemberRoleDto companyMemberRoleDto)
        {
            try
            {
                var companyMemberRole = _mapper.Map<CompanyMemberRole>(companyMemberRoleDto);
                var result = await _companyMemberRolesService.CreateCompanyMemberRoleAsync(companyMemberRole);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyMemberRoleById), new { id = result.Data.Id }, _mapper.Map<CompanyMemberRoleDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompanyMemberRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyMemberRole(string id, [FromBody] UpdateCompanyMemberRoleDto companyMemberRoleDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != companyMemberRoleDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var companyMemberRole = _mapper.Map<CompanyMemberRole>(companyMemberRoleDto);
                var result = await _companyMemberRolesService.UpdateCompanyMemberRoleAsync(companyMemberRole);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCompanyMemberRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyMemberRole(string id)
        {
            try
            {
                var result = await _companyMemberRolesService.DeleteCompanyMemberRoleAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompanyMemberRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyMemberRolesByCompanyId(string companyId)
        {
            try
            {
                var result = await _companyMemberRolesService.GetCompanyMemberRolesByCompanyIdAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyMemberRoleDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyMemberRolesByCompanyId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

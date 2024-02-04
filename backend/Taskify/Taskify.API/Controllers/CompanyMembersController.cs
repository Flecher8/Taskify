using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyMembersController : ControllerBase
    {
        private readonly ICompanyMembersService _companyMembersService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyMembersController> _logger;

        public CompanyMembersController(ICompanyMembersService companyMembersService, IMapper mapper, ILogger<CompanyMembersController> logger)
        {
            _companyMembersService = companyMembersService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyMember([FromBody] CreateCompanyMemberDto companyMemberDto)
        {
            try
            {
                var companyMember = _mapper.Map<CompanyMember>(companyMemberDto);
                var result = await _companyMembersService.CreateCompanyMemberAsync(companyMember);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyMemberById), new { id = result.Data.Id }, _mapper.Map<CompanyMemberDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompanyMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyMember(string id, [FromBody] UpdateCompanyMemberDto companyMemberDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != companyMemberDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var companyMember = _mapper.Map<CompanyMember>(companyMemberDto);
                var result = await _companyMembersService.UpdateCompanyMemberAsync(companyMember);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCompanyMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyMember(string id)
        {
            try
            {
                var result = await _companyMembersService.DeleteCompanyMemberAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompanyMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetMembersByCompanyId(string companyId)
        {
            try
            {
                var result = await _companyMembersService.GetMembersByCompanyIdAsync(companyId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyMemberDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMembersByCompanyId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetMembersByRoleId(string roleId)
        {
            try
            {
                var result = await _companyMembersService.GetMembersByRoleIdAsync(roleId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyMemberDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMembersByRoleId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyMemberById(string id)
        {
            try
            {
                var result = await _companyMembersService.GetCompanyMemberByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyMemberDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyMemberById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

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
    public class ProjectRolesController : ControllerBase
    {
        private readonly IProjectRolesService _projectRolesService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectRolesController> _logger;

        public ProjectRolesController(IProjectRolesService projectRolesService, IMapper mapper, ILogger<ProjectRolesController> logger)
        {
            _projectRolesService = projectRolesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectRoleById(string id)
        {
            try
            {
                var result = await _projectRolesService.GetProjectRoleByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectRoleDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectRoleById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetRolesByProjectId(string projectId)
        {
            try
            {
                var result = await _projectRolesService.GetRolesByProjectIdAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectRoleDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetRolesByProjectId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectRole([FromBody] CreateProjectRoleDto projectRoleDto)
        {
            try
            {
                var projectRole = _mapper.Map<ProjectRole>(projectRoleDto);
                var result = await _projectRolesService.CreateProjectRoleAsync(projectRole);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetProjectRoleById), new { id = result.Data.Id }, _mapper.Map<ProjectRoleDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateProjectRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectRole(string id, [FromBody] UpdateProjectRoleDto projectRoleDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != projectRoleDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var projectRole = _mapper.Map<ProjectRole>(projectRoleDto);
                var result = await _projectRolesService.UpdateProjectRoleAsync(projectRole);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateProjectRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectRole(string id)
        {
            try
            {
                var result = await _projectRolesService.DeleteProjectRoleAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteProjectRole method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

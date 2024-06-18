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
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMembersService _projectMembersService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectMembersController> _logger;

        public ProjectMembersController(IProjectMembersService projectMembersService, IMapper mapper, ILogger<ProjectMembersController> logger)
        {
            _projectMembersService = projectMembersService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectMember([FromBody] CreateProjectMemberDto projectMemberDto)
        {
            try
            {
                var projectMember = _mapper.Map<ProjectMember>(projectMemberDto);
                var result = await _projectMembersService.CreateProjectMemberAsync(projectMember);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetProjectMemberById), new { id = result.Data.Id }, _mapper.Map<ProjectMemberDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateProjectMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectMember(string id, [FromBody] UpdateProjectMemberDto projectMemberDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != projectMemberDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var projectMember = _mapper.Map<ProjectMember>(projectMemberDto);
                var result = await _projectMembersService.UpdateProjectMemberAsync(projectMember);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateProjectMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectMember(string id)
        {
            try
            {
                var result = await _projectMembersService.DeleteProjectMemberAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteProjectMember method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetMembersByProjectId(string projectId)
        {
            try
            {
                var result = await _projectMembersService.GetMembersByProjectIdAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectMemberDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMembersByProjectId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetMembersByRoleId(string roleId)
        {
            try
            {
                var result = await _projectMembersService.GetMembersByRoleIdAsync(roleId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectMemberDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMembersByRoleId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("role/user/{userId}")]
        public async Task<IActionResult> GetRoleByUserId(string userId)
        {
            try
            {
                var result = await _projectMembersService.GetRoleByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectRoleDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetRoleByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/projects/")]
        public async Task<IActionResult> GetProjectsByUserId(string userId)
        {
            try
            {
                var result = await _projectMembersService.GetProjectsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectMemberById(string id)
        {
            try
            {
                var result = await _projectMembersService.GetProjectMemberByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectMemberDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectMemberById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("leave/user/{userId}/project/{projectId}")]
        public async Task<IActionResult> LeaveProject(string userId, string projectId)
        {
            try
            {
                var result = await _projectMembersService.LeaveProjectByUserIdAsync(userId, projectId);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in LeaveProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

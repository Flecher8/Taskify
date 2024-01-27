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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectsService projectsService, IMapper mapper, ILogger<ProjectsController> logger)
        {
            _projectsService = projectsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            try
            {
                var result = await _projectsService.GetProjectByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            try
            {
                var project = _mapper.Map<Project>(projectDto);
                var result = await _projectsService.CreateProjectAsync(project);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetProjectById), new { id = result.Data.Id }, _mapper.Map<ProjectDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] UpdateProjectDto projectDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != projectDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var project = _mapper.Map<Project>(projectDto);
                var result = await _projectsService.UpdateProjectAsync(project);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            try
            {
                var result = await _projectsService.DeleteProjectAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectsByUserId(string userId)
        {
            try
            {
                var result = await _projectsService.GetProjectsByUserIdAsync(userId);

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
    }
}

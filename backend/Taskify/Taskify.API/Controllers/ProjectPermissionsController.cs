using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPermissionsController : ControllerBase
    {
        private readonly IProjectPermissionService _projectPermissionService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectPermissionsController> _logger;

        public ProjectPermissionsController(IProjectPermissionService projectPermissionService, IMapper mapper, ILogger<ProjectPermissionsController> logger)
        {
            _projectPermissionService = projectPermissionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("can-view")]
        public async Task<IActionResult> CanViewProjectAsync(string userId, string projectId)
        {
            try
            {
                var result = await _projectPermissionService.CanViewProjectAsync(userId, projectId);
                return result.IsSuccess
                    ? Ok(new { Result = result.Data })
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CanViewProjectAsync method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("can-edit")]
        public async Task<IActionResult> CanEditProjectAsync(string userId, string projectId)
        {
            try
            {
                var result = await _projectPermissionService.CanEditProjectAsync(userId, projectId);
                return result.IsSuccess
                    ? Ok(new { Result = result.Data })
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CanEditProjectAsync method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("can-edit-settings")]
        public async Task<IActionResult> CanEditProjectSettingsAsync(string userId, string projectId)
        {
            try
            {
                var result = await _projectPermissionService.CanEditProjectSettingsAsync(userId, projectId);
                return result.IsSuccess
                    ? Ok(new { Result = result.Data })
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CanEditProjectSettingsAsync method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

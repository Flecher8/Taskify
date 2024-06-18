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
    public class ProjectInvitationsController : ControllerBase
    {
        private readonly IProjectInvitationsService _projectInvitationService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectInvitationsController> _logger;

        public ProjectInvitationsController(IProjectInvitationsService projectInvitationService, IMapper mapper, ILogger<ProjectInvitationsController> logger)
        {
            _projectInvitationService = projectInvitationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateProjectInvitation(string userId, [FromBody] CreateProjectInvitationDto projectInvitationDto)
        {
            try
            {
                var projectInvitation = _mapper.Map<ProjectInvitation>(projectInvitationDto);
                var result = await _projectInvitationService.CreateProjectInvitationAsync(userId, projectInvitation);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetProjectInvitationById), new { id = result.Data.Id }, _mapper.Map<ProjectInvitationDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateProjectInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{projectInvitationId}/accepted/{isAccepted}")]
        public async Task<IActionResult> RespondToProjectInvitation(string projectInvitationId, bool isAccepted)
        {
            try
            {
                var result = await _projectInvitationService.RespondToProjectInvitationAsync(projectInvitationId, isAccepted);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RespondToProjectInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectInvitationsByUserId(string userId)
        {
            try
            {
                var result = await _projectInvitationService.GetProjectInvitationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectInvitationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectInvitationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadProjectInvitationsByUserId(string userId)
        {
            try
            {
                var result = await _projectInvitationService.GetUnreadProjectInvitationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectInvitationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUnreadProjectInvitationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/markread")]
        public async Task<IActionResult> MarkProjectInvitationAsRead(string id)
        {
            try
            {
                var result = await _projectInvitationService.MarkProjectInvitationAsReadAsync(id);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MarkProjectInvitationAsRead method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectInvitation(string id)
        {
            try
            {
                var result = await _projectInvitationService.DeleteProjectInvitationAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteProjectInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectInvitationById(string id)
        {
            try
            {
                var result = await _projectInvitationService.GetProjectInvitationByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectInvitationDto>(result.Data))
                    : NotFound(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectInvitationById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("notification/{notificationId}")]
        public async Task<IActionResult> GetProjectInvitationByNotificationId(string notificationId)
        {
            try
            {
                var result = await _projectInvitationService.GetProjectInvitationByNotificationIdAsync(notificationId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<ProjectInvitationDto>(result.Data))
                    : NotFound(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectInvitationById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

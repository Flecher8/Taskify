using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyInvitationsController : ControllerBase
    {
        private readonly ICompanyInvitationsService _companyInvitationService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyInvitationsController> _logger;

        public CompanyInvitationsController(ICompanyInvitationsService companyInvitationService, IMapper mapper, ILogger<CompanyInvitationsController> logger)
        {
            _companyInvitationService = companyInvitationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateCompanyInvitation(string userId, [FromBody] CreateCompanyInvitationDto companyInvitationDto)
        {
            try
            {
                var companyInvitation = _mapper.Map<CompanyInvitation>(companyInvitationDto);
                var result = await _companyInvitationService.CreateCompanyInvitationAsync(userId, companyInvitation);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCompanyInvitationById), new { id = result.Data.Id }, _mapper.Map<CompanyInvitationDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCompanyInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{companyInvitationId}/respond/{isAccepted}")]
        public async Task<IActionResult> RespondToCompanyInvitation(string companyInvitationId, bool isAccepted)
        {
            try
            {
                var result = await _companyInvitationService.RespondToCompanyInvitationAsync(companyInvitationId, isAccepted);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RespondToCompanyInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCompanyInvitationsByUserId(string userId)
        {
            try
            {
                var result = await _companyInvitationService.GetCompanyInvitationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyInvitationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyInvitationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadCompanyInvitationsByUserId(string userId)
        {
            try
            {
                var result = await _companyInvitationService.GetUnreadCompanyInvitationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CompanyInvitationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUnreadCompanyInvitationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/markread")]
        public async Task<IActionResult> MarkCompanyInvitationAsRead(string id)
        {
            try
            {
                var result = await _companyInvitationService.MarkCompanyInvitationAsReadAsync(id);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MarkCompanyInvitationAsRead method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyInvitation(string id)
        {
            try
            {
                var result = await _companyInvitationService.DeleteCompanyInvitationAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCompanyInvitation method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyInvitationById(string id)
        {
            try
            {
                var result = await _companyInvitationService.GetCompanyInvitationByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyInvitationDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyInvitationById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("notification/{notificationId}")]
        public async Task<IActionResult> GetCompanyInvitationByNotificationId(string notificationId)
        {
            try
            {
                var result = await _companyInvitationService.GetCompanyInvitationByNotificationIdAsync(notificationId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CompanyInvitationDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyInvitationByNotificationId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

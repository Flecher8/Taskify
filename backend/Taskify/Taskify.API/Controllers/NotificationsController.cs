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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService, IMapper mapper, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            try
            {
                var result = await _notificationService.GetNotificationByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<NotificationDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetNotificationById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto notificationDto)
        {
            try
            {
                var notification = _mapper.Map<Notification>(notificationDto);
                var result = await _notificationService.CreateNotificationAsync(notification);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetNotificationById), new { id = result.Data.Id }, _mapper.Map<NotificationDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateNotification method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(string id)
        {
            try
            {
                var result = await _notificationService.MarkNotificationAsReadAsync(id);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MarkNotificationAsRead method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(string userId)
        {
            try
            {
                var result = await _notificationService.GetNotificationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<NotificationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetNotificationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadNotificationsByUserId(string userId)
        {
            try
            {
                var result = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<NotificationDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUnreadNotificationsByUserId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            try
            {
                var result = await _notificationService.DeleteNotificationAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteNotification method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

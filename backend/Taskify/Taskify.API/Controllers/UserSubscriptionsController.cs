using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.Dtos;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSubscriptionsController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserSubscriptionsController> _logger;

        public UserSubscriptionsController(IUserSubscriptionService userSubscriptionService, IMapper mapper, ILogger<UserSubscriptionsController> logger)
        {
            _userSubscriptionService = userSubscriptionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserSubscription(string userId)
        {
            try
            {
                var result = await _userSubscriptionService.GetUserSubscription(userId);


                return result.IsSuccess
                    ? result.Data == null ? Ok(null) : Ok(_mapper.Map<SubscriptionDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUserSubscription method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserSubscription([FromBody] CreateUserSubscriptionDto createUserSubscriptionDto)
        {
            try
            {
                var result = await _userSubscriptionService.CreateUserSubscription(createUserSubscriptionDto.UserId, createUserSubscriptionDto.SubscriptionId);

                return result.IsSuccess
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateUserSubscription method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

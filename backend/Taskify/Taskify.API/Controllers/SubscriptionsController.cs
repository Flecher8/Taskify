using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionsController> _logger; 

        public SubscriptionsController(ISubscriptionService subscriptionService, IMapper mapper, ILogger<SubscriptionsController> logger)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            try
            {
                var result = await _subscriptionService.GetAllSubscriptionsAsync();

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<SubscriptionDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllSubscriptions method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById(string id)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<SubscriptionDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetSubscriptionById method with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto subscriptionDto)
        {
            try
            {
                var subscription = _mapper.Map<Subscription>(subscriptionDto);
                var result = await _subscriptionService.CreateSubscriptionAsync(subscription);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetSubscriptionById), new { id = result.Data.Id }, _mapper.Map<SubscriptionDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateSubscription method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(string id, [FromBody] SubscriptionDto subscriptionDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != subscriptionDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var subscription = _mapper.Map<Subscription>(subscriptionDto);

                var result = await _subscriptionService.UpdateSubscriptionAsync(subscription);

                return result.IsSuccess
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in UpdateSubscription method with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(string id)
        {
            try
            {
                var result = await _subscriptionService.DeleteSubscriptionAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteSubscription method with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

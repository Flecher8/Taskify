using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var result = await _subscriptionService.GetAllSubscriptionsAsync();

            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById(string id)
        {
            var result = await _subscriptionService.GetSubscriptionByIdAsync(id);

            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] Subscription subscription)
        {
            var result = await _subscriptionService.CreateSubscriptionAsync(subscription);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetSubscriptionById), new { id = result.Data.Id }, result.Data)
                : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(string id, [FromBody] Subscription subscription)
        {
            // Ensure the id in the path matches the id in the request body
            if (id != subscription.Id)
            {
                return BadRequest("The provided id in the path does not match the id in the request body.");
            }

            var result = await _subscriptionService.UpdateSubscriptionAsync(subscription);

            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(string id)
        {
            var result = await _subscriptionService.DeleteSubscriptionAsync(id);

            return result.IsSuccess
                ? NoContent() // Use NoContent for successful deletion
                : BadRequest(result.Errors);
        }
    }
}

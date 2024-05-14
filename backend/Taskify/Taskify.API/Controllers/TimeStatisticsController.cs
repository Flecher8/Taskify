using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Helpers;
using Taskify.BLL.Interfaces;
using Taskify.Core.Dtos.Statistics;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeStatisticsController : ControllerBase
    {
        private readonly ITimeStatisticsService _timeStatisticsService;
        private readonly IMapper _mapper;
        private readonly ILogger<TimeStatisticsController> _logger;

        public TimeStatisticsController(
            ITimeStatisticsService timeStatisticsService, 
            IMapper mapper, 
            ILogger<TimeStatisticsController> logger
        )
        {
            _timeStatisticsService = timeStatisticsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("user-project-time-statistics")]
        public async Task<IActionResult> GetUserProjectTimeStatistics([FromQuery] UserTimeSpendOnDateRequestDto requestDto)
        {
            try
            {
                // Map DTO to request object
                var request = _mapper.Map<UserTimeSpendOnDateRequest>(requestDto);

                var result = await _timeStatisticsService.GetUserProjectTimeStatisticsAsync(request);

                return result.IsSuccess
                    ? Ok(_mapper.Map<UserTimeSpendOnDateStatisticDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUserProjectTimeStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

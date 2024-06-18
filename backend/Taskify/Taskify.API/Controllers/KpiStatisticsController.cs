using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.Dtos.Statistics;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KpiStatisticsController : ControllerBase
    {
        private readonly IKpiStatisticsService _kpiStatisticsService;
        private readonly IMapper _mapper;
        private readonly ILogger<KpiStatisticsController> _logger;

        public KpiStatisticsController(
            IKpiStatisticsService kpiStatisticsService,
            IMapper mapper,
            ILogger<KpiStatisticsController> logger
        )
        {
            _kpiStatisticsService = kpiStatisticsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("project/{projectId}/user/{userId}/kpi")]
        public async Task<IActionResult> GetKpiForUser(string userId, string projectId)
        {
            try
            {
                var result = await _kpiStatisticsService.GetKpiForUserAsync(userId, projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<KpiStatisticsDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetKpiForUser method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}/kpi")]
        public async Task<IActionResult> GetKpiForAllUsersInProject(string projectId)
        {
            try
            {
                var result = await _kpiStatisticsService.GetKpiForAllUsersInProjectAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<KpiStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetKpiForAllUsersInProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

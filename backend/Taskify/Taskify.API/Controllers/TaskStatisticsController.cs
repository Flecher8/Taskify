using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taskify.BLL.Interfaces;
using Taskify.Core.Dtos.Statistics;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatisticsController : ControllerBase
    {
        private readonly ITaskStatisticsService _taskStatisticsService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskStatisticsController> _logger;

        public TaskStatisticsController(ITaskStatisticsService taskStatisticsService, IMapper mapper, ILogger<TaskStatisticsController> logger)
        {
            _taskStatisticsService = taskStatisticsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("project/{projectId}/section-type-task-count")]
        public async Task<IActionResult> GetSectionTypeTaskCountForProjectStatistics(string projectId)
        {
            try
            {
                var result = await _taskStatisticsService.GetSectionTypeTaskCountForProjectStatisticsAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<SectionTypeTaskCountStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetSectionTypeTaskCountForProjectStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}/section-task-count")]
        public async Task<IActionResult> GetTaskCountForSections(string projectId)
        {
            try
            {
                var result = await _taskStatisticsService.GetTaskCountForSectionsAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<SectionTaskCountStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTaskCountForSections method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}/user-task-count")]
        public async Task<IActionResult> GetUserTaskCountForProjectStatistics(string projectId)
        {
            try
            {
                var result = await _taskStatisticsService.GetUserTaskCountForProjectStatisticsAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<UserTaskCountStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUserTaskCountForProjectStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}/user-story-points-count")]
        public async Task<IActionResult> GetUserStoryPointsCountForProjectStatistics(string projectId)
        {
            try
            {
                var result = await _taskStatisticsService.GetUserStoryPointsCountForProjectStatisticsAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<UserStoryPointsCountStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetUserStoryPointsCountForProjectStatistics method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}/task-count-by-roles")]
        public async Task<IActionResult> GetTaskCountByRolesAsync(string projectId)
        {
            try
            {
                var result = await _taskStatisticsService.GetTaskCountByRolesAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectRoleTaskCountStatisticsDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTaskCountByRolesAsync method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

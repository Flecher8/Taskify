using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTimeTrackersController : ControllerBase
    {
        private readonly ITaskTimeTrackersService _taskTimeTrackersService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskTimeTrackersController> _logger;

        public TaskTimeTrackersController(ITaskTimeTrackersService taskTimeTrackersService, IMapper mapper, ILogger<TaskTimeTrackersController> logger)
        {
            _taskTimeTrackersService = taskTimeTrackersService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskTimeTracker([FromBody] TaskTimeTrackerDto taskTimeTrackerDto)
        {
            try
            {
                var taskTimeTracker = _mapper.Map<TaskTimeTracker>(taskTimeTrackerDto);
                var result = await _taskTimeTrackersService.CreateTaskTimeTrackerAsync(taskTimeTracker);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetTaskTimeTrackerById), new { id = result.Data.Id }, _mapper.Map<TaskTimeTrackerDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateTaskTimeTracker method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskTimeTrackerById(string id)
        {
            try
            {
                var result = await _taskTimeTrackersService.GetTaskTimeTrackerByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<TaskTimeTrackerDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTaskTimeTrackerById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskTimeTracker(string id, [FromBody] TaskTimeTrackerDto taskTimeTrackerDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != taskTimeTrackerDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var taskTimeTracker = _mapper.Map<TaskTimeTracker>(taskTimeTrackerDto);
                var result = await _taskTimeTrackersService.UpdateTaskTimeTrackerAsync(taskTimeTracker);

                return result.IsSuccess
                    ? Ok(_mapper.Map<TaskTimeTrackerDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateTaskTimeTracker method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskTimeTracker(string id)
        {
            try
            {
                var result = await _taskTimeTrackersService.DeleteTaskTimeTrackerAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteTaskTimeTracker method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("timer-start/{userId}/{taskId}")]
        public async Task<IActionResult> StartTimer(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.StartTimerAsync(userId, taskId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<TaskTimeTrackerDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in StartTimer method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("timer-stop/{userId}/{taskId}")]
        public async Task<IActionResult> StopTimer(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.StopTimerAsync(userId, taskId);

                return result.IsSuccess
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in StopTimer method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/task/{taskId}")]
        public async Task<IActionResult> GetTaskTimeTrackerByUserForTask(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.GetTaskTimeTrackerByUserForTaskAsync(userId, taskId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<TaskTimeTrackerDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTaskTimeTrackerByUserForTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

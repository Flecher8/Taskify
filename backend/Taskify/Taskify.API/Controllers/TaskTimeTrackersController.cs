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
        public async Task<IActionResult> CreateTaskTimeTracker([FromBody] CreateTaskTimeTrackerDto taskTimeTrackerDto)
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
        public async Task<IActionResult> UpdateTaskTimeTracker(string id, [FromBody] UpdateTaskTimeTracker taskTimeTrackerDto)
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
        public async Task<IActionResult> GetTaskTimeTrackersByUserForTask(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.GetTaskTimeTrackersByUserForTaskAsync(userId, taskId);

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

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskTimeTrackersByTask(string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.GetTaskTimeTrackersByTaskAsync(taskId);

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

        [HttpGet("task/{taskId}/total-seconds")]
        public async Task<IActionResult> GetTotalSecondsSpentOnTask(string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.GetNumberOfSecondsSpendOnTask(taskId);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTotalSecondsSpentOnTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user/{userId}/task/{taskId}/active-timer")]
        public async Task<IActionResult> IsTimerActive(string userId, string taskId)
        {
            try
            {
                var result = await _taskTimeTrackersService.IsTimerActiveAsync(userId, taskId);

                return result.IsSuccess 
                    ? (result.Data == null ? Ok(null) : Ok(_mapper.Map<TaskTimeTrackerDto>(result.Data)))
                    : BadRequest(result.Errors); // Return NotFound if no active timer found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in IsTimerActive method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

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
    public class CustomTasksController : ControllerBase
    {
        private readonly ICustomTasksService _customTasksService;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomTasksController> _logger;

        public CustomTasksController(ICustomTasksService customTasksService, IMapper mapper, ILogger<CustomTasksController> logger)
        {
            _customTasksService = customTasksService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomTask([FromBody] CreateCustomTaskDto createCustomTaskDto)
        {
            try
            {
                var customTask = _mapper.Map<CustomTask>(createCustomTaskDto);
                var result = await _customTasksService.CreateCustomTaskAsync(customTask);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetCustomTaskById), new { id = result.Data.Id }, _mapper.Map<CustomTaskDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomTaskById(string id)
        {
            try
            {
                var result = await _customTasksService.GetCustomTaskByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<CustomTaskDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCustomTaskById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("section/{sectionId}")]
        public async Task<IActionResult> GetCustomTasksBySectionId(string sectionId)
        {
            try
            {
                var result = await _customTasksService.GetCustomTasksBySectionIdAsync(sectionId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CustomTaskDto>>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCustomTasksBySectionId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomTask(string id, [FromBody] UpdateCustomTaskDto updateCustomTaskDto)
        {
            try
            {
                if (id != updateCustomTaskDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var customTask = _mapper.Map<CustomTask>(updateCustomTaskDto);
                var result = await _customTasksService.UpdateCustomTaskAsync(customTask);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomTask(string id)
        {
            try
            {
                var result = await _customTasksService.DeleteCustomTaskAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveCustomTask(string id)
        {
            try
            {
                var result = await _customTasksService.ArchiveCustomTaskAsync(id);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in ArchiveCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}/unarchive")]
        public async Task<IActionResult> UnarchiveCustomTask(string id)
        {
            try
            {
                var result = await _customTasksService.UnarchiveCustomTaskAsync(id);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UnarchiveCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("move")]
        public async Task<IActionResult> MoveCustomTask([FromBody] MoveCustomTaskDto moveCustomTaskDto)
        {
            try
            {
                var result = await _customTasksService.MoveCustomTaskAsync(moveCustomTaskDto.Id, moveCustomTaskDto.TargetSequenceNumber);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MoveCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("archived/{projectId}")]
        public async Task<IActionResult> GetArchivedCustomTasksByProject(string projectId)
        {
            try
            {
                var result = await _customTasksService.GetArchivedCustomTasksByProjectAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<CustomTaskDto>>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetArchivedCustomTasksByProject method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("redirect")]
        public async Task<IActionResult> RedirectCustomTask([FromBody] RedirectCustomTaskDto redirectCustomTaskDto)
        {
            try
            {
                var result = await _customTasksService.RedirectCustomTaskAsync(
                    redirectCustomTaskDto.Id,
                    redirectCustomTaskDto.TargetSectionId,
                    redirectCustomTaskDto.TargetSequenceNumber);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RedirectCustomTask method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

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
    public class ProjectIncomesController : ControllerBase
    {
        private readonly IProjectIncomesService _projectIncomesService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectIncomesController> _logger;

        public ProjectIncomesController(
            IProjectIncomesService projectIncomesService, 
            IMapper mapper, 
            ILogger<ProjectIncomesController> logger
            )
        {
            _projectIncomesService = projectIncomesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectIncomesByProjectId(string projectId)
        {
            try
            {
                var result = await _projectIncomesService.GetProjectIncomesByProjectIdAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<ProjectIncomeDto>>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProjectIncomeByProjectId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectIncome([FromBody] CreateProjectIncomeDto projectIncomeDto)
        {
            try
            {
                var projectIncome = _mapper.Map<ProjectIncome>(projectIncomeDto);
                var result = await _projectIncomesService.CreateProjectIncomeAsync(projectIncome);

                return result.IsSuccess
                    ? CreatedAtAction(
                        nameof(GetProjectIncomesByProjectId), 
                        new { projectId = result.Data.Project.Id }, 
                        _mapper.Map<ProjectIncomeDto>(result.Data)
                    )
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateProjectIncome method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectIncome(string id, [FromBody] UpdateProjectIncomeDto projectIncomeDto)
        {
            try
            {
                // Ensure the id in the path matches the id in the request body
                if (id != projectIncomeDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var projectIncome = _mapper.Map<ProjectIncome>(projectIncomeDto);
                var result = await _projectIncomesService.UpdateProjectIncomeAsync(projectIncome);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateProjectIncome method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectIncome(string id)
        {
            try
            {
                var result = await _projectIncomesService.DeleteProjectIncomeAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteProjectIncome method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

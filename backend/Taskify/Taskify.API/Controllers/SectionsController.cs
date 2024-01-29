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
    public class SectionsController : ControllerBase
    {
        private readonly ISectionsService _sectionsService;
        private readonly IMapper _mapper;
        private readonly ILogger<SectionsController> _logger;

        public SectionsController(ISectionsService sectionsService, IMapper mapper, ILogger<SectionsController> logger)
        {
            _sectionsService = sectionsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDto createSectionDto)
        {
            try
            {
                var section = _mapper.Map<Section>(createSectionDto);
                var result = await _sectionsService.CreateSectionAsync(section);

                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetSectionById), new { id = result.Data.Id }, _mapper.Map<SectionDto>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateSection method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionById(string id)
        {
            try
            {
                var result = await _sectionsService.GetSectionByIdAsync(id);

                return result.IsSuccess
                    ? Ok(_mapper.Map<SectionDto>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetSectionById method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetSectionsByProjectId(string projectId)
        {
            try
            {
                var result = await _sectionsService.GetSectionsByProjectIdAsync(projectId);

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<SectionDto>>(result.Data))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetSectionsByProjectId method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSection(string id, [FromBody] UpdateSectionDto updateSectionDto)
        {
            try
            {
                if (id != updateSectionDto.Id)
                {
                    return BadRequest("The provided id in the path does not match the id in the request body.");
                }

                var section = _mapper.Map<Section>(updateSectionDto);
                var result = await _sectionsService.UpdateSectionAsync(section);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateSection method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(string id)
        {
            try
            {
                var result = await _sectionsService.DeleteSectionAsync(id);

                return result.IsSuccess
                    ? NoContent()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteSection method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("move")]
        public async Task<IActionResult> MoveSection([FromBody] MoveSectionDto moveSectionDto)
        {
            try
            {
                var result = await _sectionsService.MoveSectionAsync(moveSectionDto.Id, moveSectionDto.MoveTo);

                return result.IsSuccess
                    ? Ok(result.Data)
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MoveSection method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Services;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;
using Taskify.DAL.Interfaces;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger) 
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();

                return result.IsSuccess
                    ? Ok(_mapper.Map<List<UserDto>>(result.Data))
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllUsers method.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpGet("id/")]
        //public async Task<IActionResult> GetUserById(string id)
        //{
            
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostUser(User user)
        //{
            
        //}

        //[HttpPut]
        //public async Task<IActionResult> PutUser(User user)
        //{
            
        //}
    }
}

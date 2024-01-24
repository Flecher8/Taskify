using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IDataRepository<User> _userRepository;

        public UsersController(IDataRepository<User> userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id/")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            var result = await _userRepository.AddAsync(user);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            await _userRepository.UpdateAsync(user);
            return Ok();
        }
    }
}

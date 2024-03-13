using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Migrations;
using Taskify.DAL.Repositories;

namespace Taskify.BLL.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IUserRepository userRepository, IValidator<User> validator, ILogger<UsersService> logger)
        {
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<bool>> DeleteUserAsync(string id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete user.");

            }
        }

        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var result = await _userRepository.GetAllAsync();
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<User>>("Can not get all users.");
            }
        }

        public async Task<Result<User>> GetUserByIdAsync(string id)
        {
            try
            {
                var result = await _userRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<User>("User with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<User>("Can not get user by id.");
            }
        }

        public async Task<Result<bool>> UpdateUserAsync(User user)
        {
            try
            {
                var userToUpdate = await _userRepository.GetByIdAsync(user.Id);

                if (userToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("User with such id does not exist.");
                }


                // Validation
                var validation = await _validator.ValidateAsync(user);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                await _userRepository.UpdateAsync(userToUpdate);
                
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update subscription.");
            }
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return ResultFactory.Failure<User>("Email can not be empty.");
                }

                var result = (await _userRepository
                    .GetFilteredItemsAsync(u => u.Email == email.ToLower()))
                    .FirstOrDefault();

                if (result == null)
                {
                    return ResultFactory.Failure<User>("User with such email does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<User>("Can not get user by email.");
            }
        }
    }
}

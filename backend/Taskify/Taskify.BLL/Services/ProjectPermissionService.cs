using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Taskify.BLL.Interfaces;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class ProjectPermissionService : IProjectPermissionService
    {
        private readonly IUserPermissionService _userPermissionService;
        private readonly ILogger<ProjectPermissionService> _logger;

        public ProjectPermissionService(IUserPermissionService userPermissionService, ILogger<ProjectPermissionService> logger)
        {
            _userPermissionService = userPermissionService;
            _logger = logger;
        }

        public async Task<Result<bool>> CanViewProjectAsync(string userId, string projectId)
        {
            try
            {
                var isMemberResult = await _userPermissionService.IsUserProjectMemberAsync(userId, projectId);
                if (!isMemberResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isMemberResult.Errors);
                }

                if (isMemberResult.Data)
                {
                    return ResultFactory.Success(true);
                }

                var isOwnerResult = await _userPermissionService.IsUserProjectOwnerAsync(userId, projectId);
                if (!isOwnerResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isOwnerResult.Errors);
                }

                return ResultFactory.Success(isOwnerResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user can view the project.");
            }
        }

        public async Task<Result<bool>> CanEditProjectAsync(string userId, string projectId)
        {
            try
            {
                var isOwnerResult = await _userPermissionService.IsUserProjectOwnerAsync(userId, projectId);
                if (!isOwnerResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isOwnerResult.Errors);
                }

                if (isOwnerResult.Data)
                {
                    return ResultFactory.Success(true);
                }

                var isMemberResult = await _userPermissionService.IsUserProjectMemberAsync(userId, projectId);
                if (!isMemberResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isMemberResult.Errors);
                }

                if (!isMemberResult.Data)
                {
                    return ResultFactory.Success(false);
                }

                var isGuestResult = await _userPermissionService.IsUserProjectRoleGuestAsync(userId, projectId);
                if (!isGuestResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isGuestResult.Errors);
                }

                if (isGuestResult.Data)
                {
                    return ResultFactory.Success(false);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user can edit the project.");
            }
        }

        public async Task<Result<bool>> CanEditProjectSettingsAsync(string userId, string projectId)
        {
            try
            {
                var isOwnerResult = await _userPermissionService.IsUserProjectOwnerAsync(userId, projectId);
                if (!isOwnerResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isOwnerResult.Errors);
                }

                if (isOwnerResult.Data)
                {
                    return ResultFactory.Success(true);
                }

                var isMemberResult = await _userPermissionService.IsUserProjectMemberAsync(userId, projectId);
                if (!isMemberResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isMemberResult.Errors);
                }

                if (!isMemberResult.Data)
                {
                    return ResultFactory.Success(false);
                }

                var isAdminResult = await _userPermissionService.IsUserProjectRoleAdminAsync(userId, projectId);
                if (!isAdminResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(isAdminResult.Errors);
                }

                return ResultFactory.Success(isAdminResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user can edit the project settings.");
            }
        }
    }
}

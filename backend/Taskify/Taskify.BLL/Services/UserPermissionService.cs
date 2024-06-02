using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.Core.Enums;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<UserPermissionService> _logger;

        public UserPermissionService(IProjectMemberRepository projectMemberRepository,
                                     IProjectRepository projectRepository,
                                     ILogger<UserPermissionService> logger)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> IsUserProjectMemberAsync(string userId, string projectId)
        {
            try
            {
                var projectMembers = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );

                return ResultFactory.Success(projectMembers.Any());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user is a project member.");
            }
        }

        public async Task<Result<bool>> IsUserProjectOwnerAsync(string userId, string projectId)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                var isOwner = project != null && project.User.Id == userId;
                return ResultFactory.Success(isOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user is the project owner.");
            }
        }

        public async Task<Result<bool>> IsUserProjectRoleAdminAsync(string userId, string projectId)
        {
            try
            {
                var projectMembers = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );
                var member = projectMembers.FirstOrDefault();
                var isAdmin = member != null && member.ProjectRole != null && member.ProjectRole.ProjectRoleType == ProjectRoleType.Admin;
                return ResultFactory.Success(isAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user has the project role admin.");
            }
        }

        public async Task<Result<bool>> IsUserProjectRoleGuestAsync(string userId, string projectId)
        {
            try
            {
                var projectMembers = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );
                var member = projectMembers.FirstOrDefault();
                var isGuest = member != null && (member.ProjectRole == null || member.ProjectRole.ProjectRoleType == ProjectRoleType.Guest);
                return ResultFactory.Success(isGuest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user has the project role guest.");
            }
        }

        public async Task<Result<bool>> IsUserProjectRoleMemberAsync(string userId, string projectId)
        {
            try
            {
                var projectMembers = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );
                var member = projectMembers.FirstOrDefault();
                var isMember = member != null && member.ProjectRole != null && member.ProjectRole.ProjectRoleType == ProjectRoleType.Member;
                return ResultFactory.Success(isMember);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Unable to check if the user has the project role member.");
            }
        }
    }
}

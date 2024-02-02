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

namespace Taskify.BLL.Services
{
    public class ProjectMembersService : IProjectMembersService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IValidator<ProjectMember> _validator;
        private readonly ILogger<ProjectMembersService> _logger;

        public ProjectMembersService(
            IProjectMemberRepository projectMemberRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IProjectRoleRepository projectRoleRepository,
            IValidator<ProjectMember> validator,
            ILogger<ProjectMembersService> logger
        )
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _projectRoleRepository = projectRoleRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<ProjectMember>> CreateProjectMemberAsync(ProjectMember projectMember)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectMember);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<ProjectMember>(validation.ErrorMessages);
                }

                if (projectMember.Project == null || string.IsNullOrEmpty(projectMember.Project.Id))
                {
                    return ResultFactory.Failure<ProjectMember>("Project is not specified.");
                }

                if (projectMember.User == null || string.IsNullOrEmpty(projectMember.User.Id))
                {
                    return ResultFactory.Failure<ProjectMember>("User is not specified.");
                }

                var project = await _projectRepository.GetByIdAsync(projectMember.Project.Id);
                var user = await _userRepository.GetByIdAsync(projectMember.User.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<ProjectMember>("Invalid project specified.");
                }

                if (user == null)
                {
                    return ResultFactory.Failure<ProjectMember>("Invalid user specified.");
                }

                projectMember.Project = project;
                projectMember.User = user;
                projectMember.ProjectRole = null;

                var result = await _projectMemberRepository.AddAsync(projectMember);
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectMember>("Can not create a new project member.");
            }
        }

        public async Task<Result<bool>> DeleteProjectMemberAsync(string id)
        {
            try
            {
                await _projectMemberRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project member.");
            }
        }

        public async Task<Result<bool>> UpdateProjectMemberAsync(ProjectMember projectMember)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectMember);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var findMemberToUpdate = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.Id == projectMember.Id)
                );

                var memberToUpdate = findMemberToUpdate.FirstOrDefault();

                if (memberToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project member with such id.");
                }

                if (projectMember.Project == null || string.IsNullOrEmpty(projectMember.Project.Id))
                {
                    return ResultFactory.Failure<bool>("Project is not specified.");
                }

                if (projectMember.User == null || string.IsNullOrEmpty(projectMember.User.Id))
                {
                    return ResultFactory.Failure<bool>("User is not specified.");
                }

                var project = await _projectRepository.GetByIdAsync(projectMember.Project.Id);
                var user = await _userRepository.GetByIdAsync(projectMember.User.Id);

                ProjectRole? projectRole = null;

                if (projectMember.ProjectRole != null && string.IsNullOrEmpty(projectMember.ProjectRole.Id))
                {
                    projectRole = await _projectRoleRepository.GetByIdAsync(projectMember.ProjectRole.Id);
                }

                if (project == null || user == null)
                {
                    return ResultFactory.Failure<bool>("Invalid project or user specified.");
                }

                memberToUpdate.Project = project;
                memberToUpdate.User = user;
                memberToUpdate.ProjectRole = projectRole;

                await _projectMemberRepository.UpdateAsync(memberToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the project member.");
            }
        }

        public async Task<Result<List<ProjectMember>>> GetMembersByProjectIdAsync(string projectId)
        {
            try
            {
                var result = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.Project.Id == projectId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectMember>>("Can not get project members by project id.");
            }
        }

        public async Task<Result<List<ProjectMember>>> GetMembersByRoleIdAsync(string roleId)
        {
            try
            {
                var result = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.ProjectRole != null && pm.ProjectRole.Id == roleId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectMember>>("Can not get project members by role id.");
            }
        }

        public async Task<Result<ProjectRole>> GetRoleByUserIdAsync(string userId)
        {
            try
            {
                var result = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.User.Id == userId)
                );

                var projectMember = result.FirstOrDefault();

                if (projectMember == null)
                {
                    return ResultFactory.Failure<ProjectRole>("Project member with such user id does not exist.");
                }

                return ResultFactory.Success(projectMember.ProjectRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectRole>("Can not get project member role by user id.");
            }
        }
    }
}

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

                var project = (await _projectRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .WithFilter(project => project.Id == projectMember.Project.Id)
                )).FirstOrDefault();

                var user = await _userRepository.GetByIdAsync(projectMember.User.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<ProjectMember>("Invalid project specified.");
                }

                if (user == null)
                {
                    return ResultFactory.Failure<ProjectMember>("Invalid user specified.");
                }

                // Check if user is creator of project
                if (project.User.Id == projectMember.User.Id)
                {
                    return ResultFactory.Failure<ProjectMember>("User is the creator of the project and cannot be added as a member.");
                }

                // Check if user is already a member of this project
                var existingMember = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .WithFilter(pm => pm.Project.Id == project.Id && pm.User.Id == user.Id)
                );

                if (existingMember.Any())
                {
                    return ResultFactory.Failure<ProjectMember>("User is already a member of this project.");
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

                ProjectRole? projectRole = null;

                if (projectMember.ProjectRole != null && !string.IsNullOrEmpty(projectMember.ProjectRole.Id))
                {
                    projectRole = await _projectRoleRepository.GetByIdAsync(projectMember.ProjectRole.Id);
                }

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
                if (string.IsNullOrEmpty(roleId))
                {
                    var results = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.ProjectRole == null)
                    );

                    return ResultFactory.Success(results);
                }
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

        public async Task<Result<ProjectMember>> GetProjectMemberByIdAsync(string id)
        {
            try
            {
                var result = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeProjectRoleEntity()
                        .WithFilter(pm => pm.Id == id) 
                    );

                var projectMember = result.FirstOrDefault();

                if (projectMember == null)
                {
                    return ResultFactory.Failure<ProjectMember>("Project member with such id does not exist.");
                }

                return ResultFactory.Success(projectMember);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectMember>("Can not get the project member by id.");
            }
        }

        public async Task<Result<List<Project>>> GetProjectsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .WithFilter(pm => pm.User.Id == userId)
                );

                var projects = result.Select(pm => pm.Project).ToList();

                return ResultFactory.Success(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Project>>("Can not get projects by user id.");
            }
        }

        public async Task<Result<bool>> IsUserAlreadyMemberAsync(string userId, string projectId)
        {
            try
            {
                var existingMemberResult = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );

                return ResultFactory.Success(existingMemberResult.Any());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Error checking if user is already a member.");
            }
        }

        public async Task<Result<bool>> LeaveProjectByUserIdAsync(string userId, string projectId)
        {
            try
            {
                var projectMembers = await _projectMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeUserEntity()
                        .WithFilter(pm => pm.User.Id == userId && pm.Project.Id == projectId)
                );

                var projectMember = projectMembers.FirstOrDefault();

                if (projectMember == null)
                {
                    return ResultFactory.Failure<bool>("Project member with the specified user ID and project ID does not exist.");
                }

                await _projectMemberRepository.DeleteAsync(projectMember.Id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Error occurred while trying to leave the project.");
            }
        }

    }
}

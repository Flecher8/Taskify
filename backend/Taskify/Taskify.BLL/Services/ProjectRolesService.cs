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
    public class ProjectRolesService : IProjectRolesService
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<ProjectRole> _validator;
        private readonly ILogger<ProjectRolesService> _logger;

        public ProjectRolesService(
            IProjectRoleRepository projectRoleRepository,
            IProjectRepository projectRepository,
            IValidator<ProjectRole> validator,
            ILogger<ProjectRolesService> logger
        )
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<ProjectRole>> CreateProjectRoleAsync(ProjectRole projectRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<ProjectRole>(validation.ErrorMessages);
                }

                if (projectRole.Project == null || string.IsNullOrEmpty(projectRole.Project.Id)) 
                {
                    return ResultFactory.Failure<ProjectRole>("Project is not specified.");
                }

                var project = await _projectRepository.GetByIdAsync(projectRole.Project.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<ProjectRole>("Can not find project with such id.");
                }

                projectRole.Project = project;

                var result = await _projectRoleRepository.AddAsync(projectRole);
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectRole>("Can not create a new project role.");
            }
        }

        public async Task<Result<ProjectRole>> GetProjectRoleByIdAsync(string id)
        {
            try
            {
                var result = await _projectRoleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<ProjectRole>("Project role with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectRole>("Can not get the project role by id.");
            }
        }

        public async Task<Result<List<ProjectRole>>> GetRolesByProjectIdAsync(string projectId)
        {
            try
            {
                var result = await _projectRoleRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .WithFilter(pr => pr.Project.Id == projectId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectRole>>("Can not get project roles by project id.");
            }
        }

        public async Task<Result<bool>> DeleteProjectRoleAsync(string id)
        {
            try
            {
                await _projectRoleRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project role.");
            }
        }

        public async Task<Result<bool>> UpdateProjectRoleAsync(ProjectRole projectRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var findRoleToUpdate = await _projectRoleRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .WithFilter(pr => pr.Id == projectRole.Id)
                );

                var roleToUpdate = findRoleToUpdate.FirstOrDefault();

                if (roleToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project role with such id.");
                }

                if (projectRole.Project == null || string.IsNullOrEmpty(projectRole.Project.Id))
                {
                    return ResultFactory.Failure<bool>("Project is not specified.");
                }

                var project = await _projectRepository.GetByIdAsync(projectRole.Project.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project with such id.");
                }

                roleToUpdate.Name = projectRole.Name;
                roleToUpdate.Project = project;
                roleToUpdate.ProjectRoleType = projectRole.ProjectRoleType;

                await _projectRoleRepository.UpdateAsync(roleToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the project role.");
            }
        }
    }
}

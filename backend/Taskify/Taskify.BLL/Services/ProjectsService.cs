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
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<Project> _validator;
        private readonly ILogger<ProjectsService> _logger;

        public ProjectsService(IProjectRepository projectRepository,
            IUserRepository userRepository,
            IValidator<Project> validator,
            ILogger<ProjectsService> logger
        )
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Project>> CreateProjectAsync(Project project)
        {
            try
            {
                if (project.User == null)
                {
                    return ResultFactory.Failure<Project>("Can not find such user id.");
                }

                var user = await _userRepository.GetByIdAsync(project.User.Id);

                if (user == null)
                {
                    return ResultFactory.Failure<Project>("Can not find such user id.");
                }

                // Validation
                var validation = await _validator.ValidateAsync(project);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Project>(validation.ErrorMessages);
                }

                project.User = user;
                project.CreatedAt = DateTime.UtcNow;

                // TODO
                // - Create starting sections for project
                // - Create starting roles for project ( give creator role for user creator)

                var result = await _projectRepository.AddAsync(project);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Project>("Can not create a new project.");
            }
        }

        public async Task<Result<bool>> DeleteProjectAsync(string id)
        {
            try
            {
                await _projectRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project.");
            }
        }

        public async Task<Result<Project>> GetProjectByIdAsync(string id)
        {
            try
            {
                var result = await _projectRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<Project>("Project with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Project>("Can not get the project by id.");
            }
        }

        public async Task<Result<bool>> UpdateProjectAsync(Project project)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(project);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var projectToUpdate = await _projectRepository.GetByIdAsync(project.Id);
                if(projectToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project with such id.");
                }

                projectToUpdate.Name = project.Name;

                await _projectRepository.UpdateAsync(projectToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the project.");
            }
        }

        public async Task<Result<List<Project>>> GetProjectsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _projectRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeUserEntity()
                                    .WithFilter(p => p.User.Id == userId)
                    );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Project>>("Can not get projects by user id.");
            }
        }
    }
}

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
    public class ProjectIncomesService : IProjectIncomesService
    {
        private readonly IProjectIncomeRepository _projectIncomeRepository;
        private readonly IProjectRepository _projectsRepository;
        private readonly IValidator<ProjectIncome> _validator;
        private readonly ILogger<ProjectIncomesService> _logger;

        public ProjectIncomesService(IProjectIncomeRepository projectIncomeRepository,
            IProjectRepository projectsRepository,
            IValidator<ProjectIncome> validator,
            ILogger<ProjectIncomesService> logger)
        {
            _projectIncomeRepository = projectIncomeRepository;
            _projectsRepository = projectsRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<ProjectIncome>> CreateProjectIncomeAsync(ProjectIncome projectIncome)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectIncome);

                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<ProjectIncome>(validation.ErrorMessages);
                }

                var projectIncomeExist = await _projectIncomeRepository.GetByIdAsync(projectIncome.Id);

                if (projectIncomeExist != null)
                {
                    return ResultFactory.Failure<ProjectIncome>("Project income with such id already exists.");
                }

                var project = await _projectsRepository.GetByIdAsync(projectIncome.Project.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<ProjectIncome>("Project with such id does not exists.");
                }

                projectIncome.Project = project;
                projectIncome.CreatedAt = DateTime.UtcNow;


                var result = await _projectIncomeRepository.AddAsync(projectIncome);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectIncome>("Can not create a new project income.");
            }
        }

        public async Task<Result<bool>> DeleteProjectIncomeAsync(string id)
        {
            try
            {
                await _projectIncomeRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project income.");
            }
        }

        public async Task<Result<List<ProjectIncome>>> GetProjectIncomesByProjectIdAsync(string projectId)
        {
            try
            {
                var result = await _projectIncomeRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeProjectEntity()
                                    .WithFilter(pi => pi.Project.Id == projectId)
                );

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectIncome>>("Can not get project incomes by project id.");
            }
        }

        public async Task<Result<bool>> UpdateProjectIncomeAsync(ProjectIncome projectIncome)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectIncome);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var projectIncomeToUpdate = await _projectIncomeRepository.GetByIdAsync(projectIncome.Id);

                if (projectIncomeToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project income with such id.");
                }

                projectIncomeToUpdate.Amount = projectIncome.Amount;
                projectIncomeToUpdate.Name = projectIncome.Name;
                projectIncomeToUpdate.Frequency = projectIncome.Frequency;

                await _projectIncomeRepository.UpdateAsync(projectIncomeToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the project income.");
            }
        }
    }
}
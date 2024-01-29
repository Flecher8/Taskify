using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class SectionsService : ISectionsService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<Section> _validator;
        private readonly ILogger<SectionsService> _logger;

        public SectionsService(
            ISectionRepository sectionRepository,
            IProjectRepository projectRepository,
            IValidator<Section> validator,
            ILogger<SectionsService> logger
        )
        {
            _sectionRepository = sectionRepository;
            _projectRepository = projectRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Section>> CreateSectionAsync(Section section)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(section);

                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Section>(validation.ErrorMessages);
                }

                var project = await _projectRepository.GetByIdAsync(section.Project.Id);

                if (project == null)
                {
                    return ResultFactory.Failure<Section>("Project with such id does not exist.");
                }

                List<Section> sectionsInCurrentProject = await _sectionRepository.GetFilteredItemsAsync(
                        builder => builder
                        .IncludeProjectEntity()
                        .WithFilter(s => s.Project.Id == project.Id)
                    );

                section.Project = project;
                section.CreatedAt = DateTime.UtcNow;
                section.SequenceNumber = sectionsInCurrentProject.Count;
                section.IsArchived = false;

                var result = await _sectionRepository.AddAsync(section);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Section>("Can not create a new section.");
            }
        }

        public async Task<Result<bool>> DeleteSectionAsync(string id)
        {
            try
            {
                await _sectionRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the section.");
            }
        }

        public async Task<Result<Section>> GetSectionByIdAsync(string id)
        {
            try
            {
                var sections = await _sectionRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCustomTasksEntity()
                        .WithFilter(s => s.Id == id)
                );

                var result = sections.FirstOrDefault();

                if (result == null)
                {
                    return ResultFactory.Failure<Section>("Section with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Section>("Can not get the section by id.");
            }
        }

        public async Task<Result<List<Section>>> GetSectionsByProjectIdAsync(string projectId)
        {
            try
            {
                var result = await _sectionRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .IncludeCustomTasksEntity()
                        .WithFilter(s => s.Project.Id == projectId)
                );

                result = result.OrderBy(s => s.SequenceNumber).ToList();

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Section>>("Can not get sections by project id.");
            }
        }

        public async Task<Result<bool>> UpdateSectionAsync(Section section)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(section);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var sectionToUpdate = await _sectionRepository.GetByIdAsync(section.Id);

                if (sectionToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find section with such id.");
                }

                // Update section properties
                sectionToUpdate.Name = section.Name;
                sectionToUpdate.SectionType = section.SectionType;
                sectionToUpdate.IsArchived = section.IsArchived;

                await _sectionRepository.UpdateAsync(sectionToUpdate);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the section.");
            }
        }

        public async Task<Result<bool>> MoveSectionAsync(string sectionId, int targetSequenceNumber)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var sectionToMove = await _sectionRepository.GetByIdAsync(sectionId);

                    if (sectionToMove == null)
                    {
                        return ResultFactory.Failure<bool>("Section with such id does not exist.");
                    }

                    var sectionsInCurrentProject = await _sectionRepository.GetFilteredItemsAsync(
                            builder => builder
                            .IncludeProjectEntity()
                            .WithFilter(s => s.Project.Id == sectionToMove.Project.Id)
                        );

                    sectionsInCurrentProject = sectionsInCurrentProject.OrderBy(s => s.SequenceNumber).ToList();

                    int currentIndex = sectionsInCurrentProject.FindIndex(s => s.Id == sectionId);

                    if (currentIndex == -1)
                    {
                        return ResultFactory.Failure<bool>("Section not found in the current project.");
                    }

                    // Ensure the target position is within bounds
                    if (!IsTargetSequenceNumberValid(targetSequenceNumber, sectionsInCurrentProject.Count))
                    {
                        return ResultFactory.Failure<bool>("Invalid target sequence number.");
                    }

                    // Move section to the target position
                    sectionsInCurrentProject.RemoveAt(currentIndex);
                    sectionsInCurrentProject.Insert(targetSequenceNumber, sectionToMove);

                    // Update sequence numbers
                    // Update sequence numbers asynchronously
                    var updateTasks = sectionsInCurrentProject.Select(async (section, index) =>
                    {
                        section.SequenceNumber = index;
                        await _sectionRepository.UpdateAsync(section);
                    });

                    await Task.WhenAll(updateTasks);

                    // If all updates are successful, commit the transaction
                    transactionScope.Complete();

                    return ResultFactory.Success(true);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    _logger.LogError(ex.Message);

                    // Roll back changes in case of an error
                    transactionScope.Dispose();
                    // Optionally, rethrow the exception if needed
                    return ResultFactory.Failure<bool>("Error while moving the section.");
                }
            }
        }

        private bool IsTargetSequenceNumberValid(int targetSequenceNumber, int sectionsCount)
        {
            return targetSequenceNumber >= 0 && targetSequenceNumber < sectionsCount;
        }
    }
}

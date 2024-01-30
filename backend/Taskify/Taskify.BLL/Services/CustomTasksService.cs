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
using Taskify.DAL.Repositories;

namespace Taskify.BLL.Services
{
    public class CustomTasksService : ICustomTasksService
    {
        private readonly ICustomTaskRepository _customTaskRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CustomTask> _validator;
        private readonly ILogger<CustomTasksService> _logger;

        public CustomTasksService(
            ICustomTaskRepository customTaskRepository,
            ISectionRepository sectionRepository,
            IUserRepository userRepository,
            IValidator<CustomTask> validator,
            ILogger<CustomTasksService> logger
        )
        {
            _customTaskRepository = customTaskRepository;
            _sectionRepository = sectionRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<CustomTask>> CreateCustomTaskAsync(CustomTask customTask)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(customTask);

                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CustomTask>(validation.ErrorMessages);
                }

                if (customTask.Section == null)
                {
                    return ResultFactory.Failure<CustomTask>("Can not find section for this task.");
                }

                var section = await _sectionRepository.GetByIdAsync(customTask.Section.Id);

                if (section == null)
                {
                    return ResultFactory.Failure<CustomTask>("Section with such id does not exist.");
                }

                List<CustomTask> tasksInCurrentSection = await _customTaskRepository.GetFilteredItemsAsync(
                        builder => builder
                        .IncludeSectionEntity()
                        .WithFilter(s => s.Section != null && s.Section.Id == section.Id)
                    );

                customTask.Section = section;
                customTask.ResponsibleUser = null;
                customTask.StartDateTimeUtc = null;
                customTask.EndDateTimeUtc = null;
                customTask.StoryPoints = null;
                customTask.IsArchived = false;
                customTask.CreatedAt = DateTime.UtcNow;
                customTask.SequenceNumber = tasksInCurrentSection.Count;

                var result = await _customTaskRepository.AddAsync(customTask);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CustomTask>("Can not create a new custom task.");
            }
        }

        public async Task<Result<bool>> DeleteCustomTaskAsync(string id)
        {
            try
            {
                var customTaskWithSection = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .WithFilter(c => c.Id == id)
                );

                var customTaskToMove = customTaskWithSection.FirstOrDefault();

                if (customTaskToMove == null)
                {
                    return ResultFactory.Failure<bool>("Custom task with such id does not exist.");
                }

                // Delete the custom task
                await _customTaskRepository.DeleteAsync(id);

                var customTasksInSection = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .WithFilter(c =>
                            c.Section != null &&
                            customTaskToMove.Section != null &&
                            c.Section.Id == customTaskToMove.Section.Id
                        )
                );

                customTasksInSection = customTasksInSection.OrderBy(c => c.SequenceNumber).ToList();

                // Update sequence numbers
                for (int index = 0; index < customTasksInSection.Count; index++)
                {
                    var task = customTasksInSection[index];
                    task.SequenceNumber = index;
                    await _customTaskRepository.UpdateAsync(task);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Error while deleting the custom task.");
            }
        }

        public async Task<Result<CustomTask>> GetCustomTaskByIdAsync(string id)
        {
            try
            {
                var customTask = await _customTaskRepository.GetByIdAsync(id);

                if (customTask == null)
                {
                    return ResultFactory.Failure<CustomTask>("Custom task with such id does not exist.");
                }

                return ResultFactory.Success(customTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CustomTask>("Can not get the custom task by id.");
            }
        }

        public async Task<Result<List<CustomTask>>> GetCustomTasksBySectionIdAsync(string sectionId)
        {
            try
            {
                var result = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .IncludeResponsibleUserEntity()
                        .WithFilter(c => c.Section != null && c.Section.Id == sectionId)
                );

                result = result.OrderBy(c => c.SequenceNumber).ToList();

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CustomTask>>("Can not get custom tasks by section id.");
            }
        }

        public async Task<Result<bool>> UpdateCustomTaskAsync(CustomTask customTask)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(customTask);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var existingCustomTask = await _customTaskRepository.GetByIdAsync(customTask.Id);

                if (existingCustomTask == null)
                {
                    return ResultFactory.Failure<bool>("Can not find custom task with such id.");
                }

                if (customTask.Section == null)
                {
                    return ResultFactory.Failure<bool>("Section can not be found.");
                }

                var existingSection = await _sectionRepository.GetByIdAsync(customTask.Section.Id);

                User? existingUser = null;

                if(customTask.ResponsibleUser != null)
                {
                    existingUser = await _userRepository.GetByIdAsync(customTask.ResponsibleUser.Id);
                }

                // Update custom task properties
                existingCustomTask.Name = customTask.Name;
                existingCustomTask.Section = existingSection;
                existingCustomTask.ResponsibleUser = existingUser;
                existingCustomTask.Description = customTask.Description;
                existingCustomTask.StartDateTimeUtc = customTask.StartDateTimeUtc;
                existingCustomTask.EndDateTimeUtc = customTask.EndDateTimeUtc;
                existingCustomTask.StoryPoints = customTask.StoryPoints;

                await _customTaskRepository.UpdateAsync(existingCustomTask);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the custom task.");
            }
        }

        public async Task<Result<bool>> MoveCustomTaskAsync(string customTaskId, int targetSequenceNumber)
        {
            try
            {

                var customTaskWithSection = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .WithFilter(c => c.Id == customTaskId)
                );

                var customTaskToMove = customTaskWithSection.FirstOrDefault();

                if (customTaskToMove == null)
                {
                    return ResultFactory.Failure<bool>("Custom task with such id does not exist.");
                }

                if (customTaskToMove.IsArchived)
                {
                    return ResultFactory.Failure<bool>("Custom task is archived.");
                }

                var customTasksInSection = await _customTaskRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeSectionEntity()
                        .WithFilter(c =>
                            c.Section != null &&
                            customTaskToMove.Section != null &&
                            c.Section.Id == customTaskToMove.Section.Id
                        )
                );

                customTasksInSection = customTasksInSection.OrderBy(c => c.SequenceNumber).ToList();

                int currentIndex = customTasksInSection.FindIndex(c => c.Id == customTaskId);

                if (currentIndex == -1)
                {
                    return ResultFactory.Failure<bool>("Custom task not found in the current section.");
                }

                // Ensure the target position is within bounds
                if (!IsTargetSequenceNumberValid(targetSequenceNumber, customTasksInSection.Count))
                {
                    return ResultFactory.Failure<bool>("Invalid target sequence number.");
                }

                // Move custom task to the target position
                customTasksInSection.RemoveAt(currentIndex);
                customTasksInSection.Insert(targetSequenceNumber, customTaskToMove);

                // Update sequence numbers
                for (int index = 0; index < customTasksInSection.Count; index++)
                {
                    var task = customTasksInSection[index];
                    task.SequenceNumber = index;
                    await _customTaskRepository.UpdateAsync(task);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogError(ex.Message);

                return ResultFactory.Failure<bool>("Error while moving the custom task.");
            }
        }

        private bool IsTargetSequenceNumberValid(int targetSequenceNumber, int tasksCount)
        {
            return targetSequenceNumber >= 0 && targetSequenceNumber < tasksCount;
        }

        public async Task<Result<bool>> ArchiveCustomTaskAsync(string customTaskId)
        {
            try
            {
                var customTask = await _customTaskRepository.GetByIdAsync(customTaskId);

                if (customTask == null)
                {
                    return ResultFactory.Failure<bool>("Custom task with such id does not exist.");
                }

                customTask.IsArchived = true;

                await _customTaskRepository.UpdateAsync(customTask);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogError(ex.Message);

                return ResultFactory.Failure<bool>("Error while archiving the custom task.");
            }
        }

        public async Task<Result<bool>> UnarchiveCustomTaskAsync(string customTaskId)
        {
            try
            {
                var customTask = await _customTaskRepository.GetByIdAsync(customTaskId);

                if (customTask == null)
                {
                    return ResultFactory.Failure<bool>("Custom task with such id does not exist.");
                }

                customTask.IsArchived = false;

                await _customTaskRepository.UpdateAsync(customTask);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogError(ex.Message);

                return ResultFactory.Failure<bool>("Error while unarchiving the custom task.");
            }
        }

        public async Task<Result<List<CustomTask>>> GetArchivedCustomTasksByProjectAsync(string projectId)
        {
            try
            {
                // Retrieve sections in the specified project
                var sectionsInProject = await _sectionRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCustomTasksEntity()
                        .IncludeProjectEntity()
                        .WithFilter(s => s.Project != null && s.Project.Id == projectId)
                );

                // Flatten the list of custom tasks from all sections
                var archivedCustomTasks = sectionsInProject
                    .SelectMany(section => section.CustomTasks)
                    .Where(customTask => customTask.IsArchived)
                    .ToList();

                return ResultFactory.Success(archivedCustomTasks);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogError(ex.Message);

                return ResultFactory.Failure<List<CustomTask>>("Error while getting archived custom tasks by project.");
            }
        }
    }
}

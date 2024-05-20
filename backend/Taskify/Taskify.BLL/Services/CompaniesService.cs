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
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<Company> _validator;
        private readonly ILogger<CompaniesService> _logger;

        public CompaniesService(ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IValidator<Company> validator,
            ILogger<CompaniesService> logger)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Company>> CreateCompanyAsync(Company company)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(company);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Company>(validation.ErrorMessages);
                }

                if (company.User == null || string.IsNullOrEmpty(company.User.Id))
                {
                    return ResultFactory.Failure<Company>("Invalid user specified.");
                }

                var user = await _userRepository.GetByIdAsync(company.User.Id);

                if (user == null)
                {
                    return ResultFactory.Failure<Company>("Can not find user with such id.");
                }

                var companyWithUser = (await _companyRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .WithFilter(c => c.User.Id == company.User.Id)
                    )).FirstOrDefault();

                if (companyWithUser != null)
                {
                    return ResultFactory.Failure<Company>("User has already created company.");
                }

                company.User = user;

                var result = await _companyRepository.AddAsync(company);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Company>("Can not create a new company.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyAsync(string id)
        {
            try
            {
                // Delete company
                await _companyRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company.");
            }
        }

        public async Task<Result<Company>> GetCompanyByIdAsync(string id)
        {
            try
            {
                var result = await _companyRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<Company>("Company with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Company>("Can not get the company by id.");
            }
        }

        public async Task<Result<bool>> UpdateCompanyAsync(Company company)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(company);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var companyToUpdate = (await _companyRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .WithFilter(c => c.Id == company.Id)
                    )).FirstOrDefault();

                if (companyToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company with such id.");
                }

                companyToUpdate.Name = company.Name;

                await _companyRepository.UpdateAsync(companyToUpdate);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the company.");
            }
        }

        public async Task<Result<Company>> GetCompaniesByUserIdAsync(string userId)
        {
            try
            {
                var result = (await _companyRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .WithFilter(c => c.User.Id == userId)
                )).FirstOrDefault();
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Company>("Can not get company by user id.");
            }
        }
    }
}

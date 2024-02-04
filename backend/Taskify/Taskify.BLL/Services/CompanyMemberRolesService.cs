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
    public class CompanyMemberRolesService : ICompanyMemberRolesService
    {
        private readonly ICompanyMemberRoleRepository _companyMemberRoleRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CompanyMemberRole> _validator;
        private readonly ILogger<CompanyMemberRolesService> _logger;

        public CompanyMemberRolesService(ICompanyMemberRoleRepository companyMemberRoleRepository,
            ICompanyMemberRepository companyMemberRepository,
            ICompanyRepository companyRepository,
            IValidator<CompanyMemberRole> validator,
            ILogger<CompanyMemberRolesService> logger)
        {
            _companyMemberRoleRepository = companyMemberRoleRepository;
            _companyMemberRepository = companyMemberRepository;
            _companyRepository = companyRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<CompanyMemberRole>> CreateCompanyMemberRoleAsync(CompanyMemberRole companyMemberRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyMemberRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CompanyMemberRole>(validation.ErrorMessages);
                }

                if (companyMemberRole.Company == null || string.IsNullOrEmpty(companyMemberRole.Company.Id))
                {
                    return ResultFactory.Failure<CompanyMemberRole>("Invalid company specified.");
                }

                var company = await _companyRepository.GetByIdAsync(companyMemberRole.Company.Id);

                if (company == null)
                {
                    return ResultFactory.Failure<CompanyMemberRole>("Can not find company by such id.");
                }

                companyMemberRole.Company = company;

                var result = await _companyMemberRoleRepository.AddAsync(companyMemberRole);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyMemberRole>("Can not create a new company member role.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyMemberRoleAsync(string id)
        {
            try
            {
                var companyMemberRole = await _companyMemberRoleRepository.GetByIdAsync(id);

                if (companyMemberRole == null)
                {
                    return ResultFactory.Failure<bool>("Company member role with such id does not exist.");
                }

                // Find all company members with the specified role
                var companyMembersWithRole = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeRoleEntity()
                        .WithFilter(cm => cm.Role != null && cm.Role.Id == id)
                );

                // Set CompanyMemberRole to null for each associated company member
                foreach (var companyMember in companyMembersWithRole)
                {
                    companyMember.Role = null;
                    await _companyMemberRepository.UpdateAsync(companyMember);
                }

                // Now, delete the company member role
                await _companyMemberRoleRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company member role.");
            }
        }

        public async Task<Result<CompanyMemberRole>> GetCompanyMemberRoleByIdAsync(string id)
        {
            try
            {
                var result = await _companyMemberRoleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<CompanyMemberRole>("Company member role with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyMemberRole>("Can not get the company member role by id.");
            }
        }

        public async Task<Result<bool>> UpdateCompanyMemberRoleAsync(CompanyMemberRole companyMemberRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyMemberRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var companyMemberRoleToUpdate = (await _companyMemberRoleRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeCompanyEntity()
                            .WithFilter(cmr => cmr.Id == companyMemberRole.Id)
                    )).FirstOrDefault();

                if (companyMemberRoleToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company member role with such id.");
                }

                companyMemberRoleToUpdate.Name = companyMemberRole.Name;

                await _companyMemberRoleRepository.UpdateAsync(companyMemberRoleToUpdate);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the company member role.");
            }
        }

        public async Task<Result<List<CompanyMemberRole>>> GetCompanyMemberRolesByCompanyIdAsync(string companyId)
        {
            try
            {
                var result = await _companyMemberRoleRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCompanyEntity()
                        .WithFilter(cmr => cmr.Company.Id == companyId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyMemberRole>>("Can not get company member roles by company id.");
            }
        }
    }
}

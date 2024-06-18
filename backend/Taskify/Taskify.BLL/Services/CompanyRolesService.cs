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
    public class CompanyRolesService : ICompanyRolesService
    {
        private readonly ICompanyRoleRepository _companyRoleRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CompanyRole> _validator;
        private readonly ILogger<CompanyRolesService> _logger;

        public CompanyRolesService(ICompanyRoleRepository companyRoleRepository,
            ICompanyMemberRepository companyMemberRepository,
            ICompanyRepository companyRepository,
            IValidator<CompanyRole> validator,
            ILogger<CompanyRolesService> logger)
        {
            _companyRoleRepository = companyRoleRepository;
            _companyMemberRepository = companyMemberRepository;
            _companyRepository = companyRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<CompanyRole>> CreateCompanyRoleAsync(CompanyRole companyRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CompanyRole>(validation.ErrorMessages);
                }

                if (companyRole.Company == null || string.IsNullOrEmpty(companyRole.Company.Id))
                {
                    return ResultFactory.Failure<CompanyRole>("Invalid company specified.");
                }

                var company = await _companyRepository.GetByIdAsync(companyRole.Company.Id);

                if (company == null)
                {
                    return ResultFactory.Failure<CompanyRole>("Can not find company by such id.");
                }

                companyRole.Company = company;

                var result = await _companyRoleRepository.AddAsync(companyRole);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyRole>("Can not create a new company member role.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyRoleAsync(string id)
        {
            try
            {
                var companyRole = await _companyRoleRepository.GetByIdAsync(id);

                if (companyRole == null)
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
                await _companyRoleRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company member role.");
            }
        }

        public async Task<Result<CompanyRole>> GetCompanyRoleByIdAsync(string id)
        {
            try
            {
                var result = await _companyRoleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<CompanyRole>("Company member role with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyRole>("Can not get the company member role by id.");
            }
        }

        public async Task<Result<bool>> UpdateCompanyRoleAsync(CompanyRole companyMemberRole)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyMemberRole);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var companyRoleToUpdate = (await _companyRoleRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeCompanyEntity()
                            .WithFilter(cmr => cmr.Id == companyMemberRole.Id)
                    )).FirstOrDefault();

                if (companyRoleToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company member role with such id.");
                }

                companyRoleToUpdate.Name = companyMemberRole.Name;

                await _companyRoleRepository.UpdateAsync(companyRoleToUpdate);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the company member role.");
            }
        }

        public async Task<Result<List<CompanyRole>>> GetCompanyRolesByCompanyIdAsync(string companyId)
        {
            try
            {
                var result = await _companyRoleRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCompanyEntity()
                        .WithFilter(cmr => cmr.Company.Id == companyId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyRole>>("Can not get company member roles by company id.");
            }
        }
    }
}

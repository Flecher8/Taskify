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
    public class CompanyMembersService : ICompanyMembersService
    {
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyRoleRepository _companyMemberRoleRepository;
        private readonly IValidator<CompanyMember> _validator;
        private readonly ILogger<CompanyMembersService> _logger;

        public CompanyMembersService(
            ICompanyMemberRepository companyMemberRepository,
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyRoleRepository companyMemberRoleRepository,
            IValidator<CompanyMember> validator,
            ILogger<CompanyMembersService> logger
        )
        {
            _companyMemberRepository = companyMemberRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyMemberRoleRepository = companyMemberRoleRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<CompanyMember>> CreateCompanyMemberAsync(CompanyMember companyMember)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyMember);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CompanyMember>(validation.ErrorMessages);
                }

                if (companyMember.User == null || string.IsNullOrEmpty(companyMember.User.Id))
                {
                    return ResultFactory.Failure<CompanyMember>("User is not specified.");
                }

                if (companyMember.Company == null || string.IsNullOrEmpty(companyMember.Company.Id))
                {
                    return ResultFactory.Failure<CompanyMember>("Company is not specified.");
                }

                var user = await _userRepository.GetByIdAsync(companyMember.User.Id);
                var company = (await _companyRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeUserEntity()
                            .WithFilter(c => c.Id == companyMember.Company.Id)
                    )).FirstOrDefault();

                if (user == null)
                {
                    return ResultFactory.Failure<CompanyMember>("Invalid user specified.");
                }

                if (company == null)
                {
                    return ResultFactory.Failure<CompanyMember>("Invalid company specified.");
                }

                // Check if the user is the creator of the company
                if (company.User.Id == user.Id)
                {
                    return ResultFactory.Failure<CompanyMember>("User is the creator of the company and cannot be added as a member.");
                }

                // Check if the user is already a member of this company
                var existingMembers = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(cm => cm.Company.Id == company.Id && cm.User.Id == user.Id)
                );

                if (existingMembers.Any())
                {
                    return ResultFactory.Failure<CompanyMember>("User is already a member of this company.");
                }

                companyMember.User = user;
                companyMember.Company = company;
                companyMember.Role = null;

                var result = await _companyMemberRepository.AddAsync(companyMember);
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyMember>("Can not create a new company member.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyMemberAsync(string id)
        {
            try
            {
                await _companyMemberRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company member.");
            }
        }

        public async Task<Result<bool>> UpdateCompanyMemberAsync(CompanyMember companyMember)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyMember);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var memberToUpdate = (await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCompanyEntity()
                        .IncludeRoleEntity()
                        .WithFilter(cm => cm.Id == companyMember.Id)
                )).FirstOrDefault();

                if (memberToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company member with such id.");
                }

                CompanyRole? companyMemberRole = null;

                if (companyMember.Role != null && !string.IsNullOrEmpty(companyMember.Role.Id))
                {
                    companyMemberRole = await _companyMemberRoleRepository.GetByIdAsync(companyMember.Role.Id);
                }

                memberToUpdate.Role = companyMemberRole;
                memberToUpdate.Salary = companyMember.Salary;

                await _companyMemberRepository.UpdateAsync(memberToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the company member.");
            }
        }

        public async Task<Result<List<CompanyMember>>> GetMembersByCompanyIdAsync(string companyId)
        {
            try
            {
                var result = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCompanyEntity()
                        .IncludeRoleEntity()
                        .WithFilter(cm => cm.Company.Id == companyId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyMember>>("Can not get company members by company id.");
            }
        }

        public async Task<Result<List<CompanyMember>>> GetMembersByRoleIdAsync(string roleId)
        {
            try
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    var results = await _companyMemberRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeRoleEntity()
                            .IncludeCompanyEntity()
                            .IncludeUserEntity()
                            .WithFilter(cm => cm.Role == null)
                    );

                    return ResultFactory.Success(results);
                }

                var result = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeRoleEntity()
                        .IncludeCompanyEntity()
                        .IncludeUserEntity()
                        .WithFilter(cm => cm.Role != null && cm.Role.Id == roleId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyMember>>("Can not get company members by role id.");
            }
        }

        public async Task<Result<CompanyMember>> GetCompanyMemberByIdAsync(string id)
        {
            try
            {
                var companyMember = (await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCompanyEntity()
                        .IncludeUserEntity()
                        .IncludeRoleEntity()
                        .WithFilter(cm => cm.Id == id)
                )).FirstOrDefault();

                if (companyMember == null)
                {
                    return ResultFactory.Failure<CompanyMember>("Company member with such id does not exist.");
                }

                return ResultFactory.Success(companyMember);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyMember>("Can not get the company member by id.");
            }
        }

        public async Task<Result<bool>> IsUserAlreadyMemberAsync(string userId, string companyId)
        {
            try
            {
                var existingMemberResult = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(cm => cm.User.Id == userId && cm.Company.Id == companyId)
                );

                return ResultFactory.Success(existingMemberResult.Any());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Error checking if user is already a member.");
            }
        }

        public async Task<Result<bool>> LeaveCompanyByUserIdAsync(string userId, string companyId)
        {
            try
            {
                var companyMember = (await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(cm => cm.User.Id == userId && cm.Company.Id == companyId)
                )).FirstOrDefault();

                if (companyMember == null)
                {
                    return ResultFactory.Failure<bool>("Company member not found.");
                }

                await _companyMemberRepository.DeleteAsync(companyMember.Id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Error occurred while trying to leave the company.");
            }
        }

        public async Task<Result<List<Company>>> GetCompaniesByUserIdAsync(string userId)
        {
            try
            {
                var result = await _companyMemberRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCompanyEntity()
                        .IncludeUserEntity()
                        .WithFilter(cm => cm.User.Id == userId)
                );

                var companies = result.Select(cm => cm.Company).ToList();

                return ResultFactory.Success(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Company>>("Can not get companies by user id.");
            }
        }

    }
}

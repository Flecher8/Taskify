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
    public class CompanyExpensesService : ICompanyExpensesService
    {
        private readonly ICompanyExpenseRepository _companyExpenseRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CompanyExpense> _validator;
        private readonly ILogger<CompanyExpensesService> _logger;

        public CompanyExpensesService(ICompanyExpenseRepository companyExpenseRepository, 
            ICompanyRepository companyRepository,
            IValidator<CompanyExpense> validator, 
            ILogger<CompanyExpensesService> logger
            )
        {
            _companyExpenseRepository = companyExpenseRepository;
            _companyRepository = companyRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<CompanyExpense>> CreateCompanyExpenseAsync(CompanyExpense companyExpense)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyExpense);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CompanyExpense>(validation.ErrorMessages);
                }

                if (companyExpense.Company == null || string.IsNullOrEmpty(companyExpense.Company.Id))
                {
                    return ResultFactory.Failure<CompanyExpense>("Invalid company specified.");
                }

                var company = await _companyRepository.GetByIdAsync(companyExpense.Company.Id);

                if (company == null)
                {
                    return ResultFactory.Failure<CompanyExpense>("Can not find company by such id.");
                }

                companyExpense.Company = company;
                companyExpense.CreatedAt = DateTime.UtcNow;

                var result = await _companyExpenseRepository.AddAsync(companyExpense);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyExpense>("Can not create a new company expense.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyExpenseAsync(string id)
        {
            try
            {
                // Delete company expense
                await _companyExpenseRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company expense.");
            }
        }

        public async Task<Result<CompanyExpense>> GetCompanyExpenseByIdAsync(string id)
        {
            try
            {
                var result = await _companyExpenseRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<CompanyExpense>("Company expense with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyExpense>("Can not get the company expense by id.");
            }
        }

        public async Task<Result<bool>> UpdateCompanyExpenseAsync(CompanyExpense companyExpense)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyExpense);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var companyExpenseToUpdate = (await _companyExpenseRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeCompanyEntity()
                            .WithFilter(ce => ce.Id == companyExpense.Id)
                    )).FirstOrDefault();

                if (companyExpenseToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company expense with such id.");
                }

                companyExpenseToUpdate.Name = companyExpense.Name;
                companyExpenseToUpdate.Amount = companyExpense.Amount;
                companyExpenseToUpdate.Frequency = companyExpense.Frequency;

                await _companyExpenseRepository.UpdateAsync(companyExpenseToUpdate);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the company expense.");
            }
        }

        public async Task<Result<List<CompanyExpense>>> GetCompanyExpensesByCompanyIdAsync(string companyId)
        {
            try
            {
                var result = await _companyExpenseRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeCompanyEntity()
                        .WithFilter(ce => ce.Company.Id == companyId)
                );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyExpense>>("Can not get company expenses by company id.");
            }
        }
    }
}

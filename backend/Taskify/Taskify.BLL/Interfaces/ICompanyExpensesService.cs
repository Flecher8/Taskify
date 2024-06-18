using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompanyExpensesService
    {
        Task<Result<CompanyExpense>> CreateCompanyExpenseAsync(CompanyExpense companyExpense);
        Task<Result<bool>> DeleteCompanyExpenseAsync(string id);
        Task<Result<CompanyExpense>> GetCompanyExpenseByIdAsync(string id);
        Task<Result<bool>> UpdateCompanyExpenseAsync(CompanyExpense companyExpense);
        Task<Result<List<CompanyExpense>>> GetCompanyExpensesByCompanyIdAsync(string companyId);
    }
}

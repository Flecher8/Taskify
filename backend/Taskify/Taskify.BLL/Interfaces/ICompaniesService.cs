using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompaniesService
    {
        Task<Result<Company>> CreateCompanyAsync(Company company);
        Task<Result<bool>> DeleteCompanyAsync(string id);
        Task<Result<Company>> GetCompanyByIdAsync(string id);
        Task<Result<bool>> UpdateCompanyAsync(Company company);
        Task<Result<Company>> GetCompanyByUserIdAsync(string userId);
    }
}

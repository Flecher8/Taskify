using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompanyRolesService
    {
        Task<Result<CompanyRole>> CreateCompanyRoleAsync(CompanyRole companyMemberRole);

        Task<Result<bool>> DeleteCompanyRoleAsync(string id);

        Task<Result<CompanyRole>> GetCompanyRoleByIdAsync(string id);

        Task<Result<bool>> UpdateCompanyRoleAsync(CompanyRole companyMemberRole);

        Task<Result<List<CompanyRole>>> GetCompanyRolesByCompanyIdAsync(string companyId);
    }
}

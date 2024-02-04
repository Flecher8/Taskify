using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompanyMemberRolesService
    {
        Task<Result<CompanyMemberRole>> CreateCompanyMemberRoleAsync(CompanyMemberRole companyMemberRole);

        Task<Result<bool>> DeleteCompanyMemberRoleAsync(string id);

        Task<Result<CompanyMemberRole>> GetCompanyMemberRoleByIdAsync(string id);

        Task<Result<bool>> UpdateCompanyMemberRoleAsync(CompanyMemberRole companyMemberRole);

        Task<Result<List<CompanyMemberRole>>> GetCompanyMemberRolesByCompanyIdAsync(string companyId);
    }
}

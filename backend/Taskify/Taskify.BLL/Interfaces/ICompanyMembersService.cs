using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompanyMembersService
    {
        Task<Result<CompanyMember>> CreateCompanyMemberAsync(CompanyMember companyMember);
        Task<Result<bool>> DeleteCompanyMemberAsync(string id);
        Task<Result<bool>> UpdateCompanyMemberAsync(CompanyMember companyMember);
        Task<Result<List<CompanyMember>>> GetMembersByCompanyIdAsync(string companyId);
        Task<Result<List<CompanyMember>>> GetMembersByRoleIdAsync(string roleId);
        Task<Result<CompanyMember>> GetCompanyMemberByIdAsync(string id);
        Task<Result<bool>> IsUserAlreadyMemberAsync(string userId, string companyId);
        Task<Result<bool>> LeaveCompanyByUserIdAsync(string userId, string companyId);
        Task<Result<List<Company>>> GetCompaniesByUserIdAsync(string userId);
    }
}

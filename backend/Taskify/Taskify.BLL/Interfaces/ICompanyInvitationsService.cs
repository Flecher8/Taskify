using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ICompanyInvitationsService
    {
        Task<Result<CompanyInvitation>> CreateCompanyInvitationAsync(string userId, CompanyInvitation companyInvitation);
        Task<Result<bool>> RespondToCompanyInvitationAsync(string companyInvitationId, bool isAccepted);
        Task<Result<List<CompanyInvitation>>> GetCompanyInvitationsByUserIdAsync(string userId);
        Task<Result<List<CompanyInvitation>>> GetUnreadCompanyInvitationsByUserIdAsync(string userId);
        Task<Result<bool>> MarkCompanyInvitationAsReadAsync(string companyInvitationId);
        Task<Result<bool>> DeleteCompanyInvitationAsync(string companyInvitationId);
        Task<Result<CompanyInvitation>> GetCompanyInvitationByIdAsync(string companyInvitationId);
        Task<Result<CompanyInvitation>> GetCompanyInvitationByNotificationIdAsync(string notificationId);
    }
}

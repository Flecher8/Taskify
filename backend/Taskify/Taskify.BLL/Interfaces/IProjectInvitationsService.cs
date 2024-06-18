using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IProjectInvitationsService
    {
        Task<Result<ProjectInvitation>> CreateProjectInvitationAsync(string userId, ProjectInvitation projectInvitation);
        Task<Result<bool>> RespondToProjectInvitationAsync(string projectInvitationId, bool isAccepted);
        Task<Result<List<ProjectInvitation>>> GetProjectInvitationsByUserIdAsync(string userId);
        Task<Result<List<ProjectInvitation>>> GetUnreadProjectInvitationsByUserIdAsync(string userId);
        Task<Result<bool>> MarkProjectInvitationAsReadAsync(string projectInvitationId);
        Task<Result<bool>> DeleteProjectInvitationAsync(string projectInvitationId);
        Task<Result<ProjectInvitation>> GetProjectInvitationByIdAsync(string projectInvitationId);
        Task<Result<ProjectInvitation>> GetProjectInvitationByNotificationIdAsync(string notificationId);
    }
}

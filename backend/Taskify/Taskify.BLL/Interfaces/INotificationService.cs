using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface INotificationService
    {
        Task<Result<Notification>> CreateNotificationAsync(Notification notification);
        Task<Result<bool>> MarkNotificationAsReadAsync(string notificationId);
        Task<Result<List<Notification>>> GetNotificationsByUserIdAsync(string userId);
        Task<Result<List<Notification>>> GetUnreadNotificationsByUserIdAsync(string userId);
        Task<Result<bool>> DeleteNotificationAsync(string notificationId);
        Task<Result<Notification>> GetNotificationByIdAsync(string notificationId);
    }
}

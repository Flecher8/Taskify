using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Interfaces
{
    public interface INotificationRepository : IDataRepository<Notification>
    {
    }
}

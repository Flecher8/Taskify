using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;

namespace Taskify.DAL.Interfaces
{
    public interface IProjectMemberRepository : IFilterableRepository<ProjectMember, ProjectMemberFilterBuilder>
    {
    }
}

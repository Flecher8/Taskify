using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CustomTaskFilterBuilder : BaseFilterBuilder<CustomTask>
    {
        public bool IncludeSection { get; private set; }
        public bool IncludeResponsibleUser { get; private set; }

        public CustomTaskFilterBuilder IncludeSectionEntity()
        {
            IncludeSection = true;
            return this;
        }

        public CustomTaskFilterBuilder IncludeResponsibleUserEntity()
        {
            IncludeResponsibleUser = true;
            return this;
        }
    }
}

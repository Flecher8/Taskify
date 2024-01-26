using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyMemberFilterBuilder
    {
        public bool IncludeUser { get; private set; }
        public bool IncludeCompany { get; private set; }
        public bool IncludeRole { get; private set; }

        public Expression<Func<CompanyMember, bool>> Filter { get; private set; } = _ => true;

        public CompanyMemberFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public CompanyMemberFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }

        public CompanyMemberFilterBuilder IncludeRoleEntity()
        {
            IncludeRole = true;
            return this;
        }

        public CompanyMemberFilterBuilder WithFilter(Expression<Func<CompanyMember, bool>> filter)
        {
            Filter = filter;
            return this;
        }
    }
}

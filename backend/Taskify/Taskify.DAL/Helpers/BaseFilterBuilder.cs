using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.DAL.Helpers
{
    public class BaseFilterBuilder<T>
    {
        protected Expression<Func<T, bool>> Filter { get; private set; } = _ => true;

        public BaseFilterBuilder<T> WithFilter(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
            return this;
        }
    }
}

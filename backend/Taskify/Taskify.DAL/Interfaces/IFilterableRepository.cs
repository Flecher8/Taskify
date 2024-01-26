using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.DAL.Interfaces
{
    public interface IFilterableRepository<T, TFilterData> : IDataRepository<T>
    {
        Task<List<T>> GetFilteredItemsAsync(Action<TFilterData> buildFilter);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.DAL.Interfaces
{
    public interface IDataRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetFilteredItemsAsync(Expression<Func<T, bool>> filter);

        Task<T?> GetById(string id);

        Task<T> AddAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(string id);
    }
}

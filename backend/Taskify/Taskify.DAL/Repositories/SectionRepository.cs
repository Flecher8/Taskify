using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.DAL.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly DataContext _dbContext;

        public SectionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Section> AddAsync(Section item)
        {
            await _dbContext.Sections.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var section = await _dbContext.Sections.FindAsync(id);
            if (section != null)
            {
                _dbContext.Sections.Remove(section);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Section>> GetAllAsync()
        {
            return await _dbContext.Sections.ToListAsync();
        }

        public async Task<Section?> GetByIdAsync(string id)
        {
            return await _dbContext.Sections.FindAsync(id);
        }

        public async Task<List<Section>> GetFilteredItemsAsync(Expression<Func<Section, bool>> filter)
        {
            return await _dbContext.Sections.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(Section item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }

}

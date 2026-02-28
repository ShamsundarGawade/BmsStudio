using BmsStudio.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Persistence.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        public Task<T?> GetByIdAsync(Guid id)
        {
            // EF Core logic
            return Task.FromResult<T?>(null);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(new List<T>());
        }

        public Task AddAsync(T entity)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            return Task.CompletedTask;
        }
    }
}

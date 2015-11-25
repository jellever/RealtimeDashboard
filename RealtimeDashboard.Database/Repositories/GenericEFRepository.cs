using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeDashboard.Database.Repositories
{
	public class GenericEFRepository<T> : IRepository<T> where T : class
	{
		protected DbContext dbContext;
		protected DbSet<T> dbSet;

		public GenericEFRepository(DbContext dbContext)
		{
			this.dbContext = dbContext;
			this.dbSet = this.dbContext.Set<T>();
		}

		public Task<T> Get(Int64 id)
		{
			return this.dbSet.FindAsync(id);
		}

		public Task<T> Get(Expression<Func<T, bool>> predicate)
		{
			return this.dbSet.SingleOrDefaultAsync(predicate, CancellationToken.None);
		}

		public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
		{
            // ReSharper disable once SuspiciousTypeConversion.Global
		    IEnumerable<T> result = await this.dbSet.Where(predicate).ToListAsync(CancellationToken.None);
		    return result;
		}

        public async Task<IEnumerable<T>> GetAll()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            IEnumerable<T> result = await this.dbSet.ToListAsync(CancellationToken.None);
            return result;
        }

        public Task Add(T entity)
		{
			this.dbSet.Add(entity);
			return Task.FromResult((object)null);
		}

		public Task Update(T entity)
		{
			this.dbContext.Entry(entity).State = EntityState.Modified;
			return Task.FromResult((object)null);
		}

		public Task Delete(T entity)
		{
			this.dbSet.Remove(entity);
			return Task.FromResult((object)null);
		}

		public void Dispose()
		{
			//emtpy
		}
	}
}

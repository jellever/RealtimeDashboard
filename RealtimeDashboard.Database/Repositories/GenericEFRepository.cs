using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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

        protected IQueryable<T> ApplyIncludes<T>(IQueryable<T> query, Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> result = query;
            if (includeExpressions != null && includeExpressions.Count() > 0)
            {
                foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
                {
                    result = result.Include(includeExpression);
                }
            }
            return result;
        }

        public async Task<T> Get(Int64 id)
        {
            T result = await this.dbSet.FindAsync(id);
            ((IObjectContextAdapter)dbContext).ObjectContext.Refresh(RefreshMode.StoreWins, result);
            return result;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = ApplyIncludes(this.dbSet, includeExpressions);
            T r = await query.SingleOrDefaultAsync(predicate, CancellationToken.None);
            return r;
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = ApplyIncludes(this.dbSet, includeExpressions);
            IEnumerable<T> result = await query.Where(predicate).ToListAsync(CancellationToken.None);
            return result;
        }

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = ApplyIncludes(this.dbSet, includeExpressions);
            IEnumerable<T> result = await query.ToListAsync(CancellationToken.None);
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

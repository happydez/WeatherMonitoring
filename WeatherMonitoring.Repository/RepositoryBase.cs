using WeatherMonitoring.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WeatherMonitoring.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            var result = repositoryContext.Set<T>();
            return !trackChanges ? result.AsNoTracking() : result;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            var result = repositoryContext.Set<T>().Where(expression);
            return !trackChanges ? result.AsNoTracking() : result;
        }

        public void Create(T entity)
        {
            repositoryContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            repositoryContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            repositoryContext.Set<T>().Update(entity);
        }
    }
}

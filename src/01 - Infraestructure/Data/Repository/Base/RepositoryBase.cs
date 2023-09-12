using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Domain.Interfaces.Repository.Base;
using Data.DataContext.Context;

namespace Data.Repository.Base
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class, new()
    {
        private readonly AppDbContext _context;
        private DbSet<TEntity> DbSet { get; }

        protected RepositoryBase(IServiceProvider service)
        {
            _context = service.GetRequiredService<AppDbContext>();
            DbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null)
                return DbSet.Where(expression);

            return DbSet.AsNoTracking();
        }

        public async Task<TEntity> GetByIdAsync(int id) => await DbSet.FindAsync(id);

        public async Task InsertAsync(TEntity entity) => await DbSet.AddAsync(entity);

        public void Update(TEntity entity) => DbSet.Update(entity);
        public void UpdateRange(List<TEntity> entity) => DbSet.UpdateRange(entity);

        public void Delete(TEntity entity) => DbSet.Remove(entity);
        public void DeleteRange(TEntity[] entityArray) => DbSet.RemoveRange(entityArray);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;     
    }
}

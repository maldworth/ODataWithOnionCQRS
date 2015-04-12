using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ODataWithOnionCQRS.Core.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}

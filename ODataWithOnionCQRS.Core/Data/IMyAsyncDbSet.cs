using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Data
{
    // Modeled similarly to IDbSet<TEntity> but with the addition of FindAsync
    public interface IMyAsyncDbSet<TEntity>: IQueryable<TEntity>, IEnumerable<TEntity>, IQueryable, IEnumerable
        where TEntity : class
    {
        ObservableCollection<TEntity> Local { get; }

        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        TEntity Attach(TEntity entity);
        TEntity Create();
        TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity;
        TEntity Find(params object[] keyValues);
        TEntity Remove(TEntity entity);
        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
        Task<TEntity> FindAsync(params object[] keyValues);
    }
}

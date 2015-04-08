using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Expressions;
using ODataWithOnionCQRS.Core.Data;
namespace ODataWithOnionCQRS.Data
{
    // Using the Adapter pattern with IMyAsyncDbSet so we can get FindAsync (because IDbSet doesn't have it)
    public class MyDbSet<TEntity> : IMyAsyncDbSet<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _innerDbSet;

        public MyDbSet(DbSet<TEntity> innerDbSet)
        {
            _innerDbSet = innerDbSet;
        }

        #region Implement Interface
        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return _innerDbSet.FindAsync(keyValues);
        }

        public TEntity Add(TEntity entity)
        {
            return _innerDbSet.Add(entity);
        }

        public TEntity Attach(TEntity entity)
        {
            return _innerDbSet.Attach(entity);
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            return _innerDbSet.Create<TDerivedEntity>();
        }

        public TEntity Create()
        {
            return _innerDbSet.Create();
        }

        public TEntity Find(params object[] keyValues)
        {
            return _innerDbSet.Find(keyValues);
        }

        public ObservableCollection<TEntity> Local
        {
            get { return _innerDbSet.Local; }
        }

        public TEntity Remove(TEntity entity)
        {
            return _innerDbSet.Remove(entity);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_innerDbSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDbSet).GetEnumerator();
        }

        public Type ElementType
        {
            get { return ((IQueryable)_innerDbSet).ElementType; }
        }

        public Expression Expression
        {
            get { return ((IQueryable)_innerDbSet).Expression; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable)_innerDbSet).Provider; }
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return _innerDbSet.AddRange(entities);
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            return _innerDbSet.RemoveRange(entities);
        }
        #endregion
    }
}

using System;
using System.Linq.Expressions;

namespace ODataWithOnionCQRS.Core.Services
{
    public interface IIncludeQuery<TEntity>
    {
        Expression<Func<TEntity, object>>[] IncludeProperties { get; }
    }
}

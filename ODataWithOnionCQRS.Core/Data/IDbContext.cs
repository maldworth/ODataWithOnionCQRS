using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Data
{
    public interface IDbContext
    {
        IMyAsyncDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}

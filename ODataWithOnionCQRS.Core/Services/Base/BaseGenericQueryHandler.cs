using MediatR;
using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Query;
using ODataWithOnionCQRS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODataWithOnionCQRS.Core.Data;
using System.Data.Entity;

namespace ODataWithOnionCQRS.Core.Services.Base
{
    public abstract class BaseGenericQueryHandler<TEntity, TDbContext> : IRequestHandler<GenericQuery<TEntity>, IEnumerable<TEntity>>
        where TEntity : class
        where TDbContext : class, IDbContext
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public BaseGenericQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public IEnumerable<TEntity> Handle(GenericQuery<TEntity> args)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                IDbContext dbCtx = dbContextScope.DbContexts.GetByInterface<TDbContext>();

                ((DbContext)dbCtx).Configuration.ProxyCreationEnabled = false;

                IQueryable<TEntity> entities = dbCtx.Set<TEntity>();

                entities = entities.Include(args);
                entities = entities.Where(args);
                entities = entities.OrderBy(args);

                // Depending on your needs, you may not want to have .Take be mandatory
                return entities.Take(args.PageSize).ToList();
            }
        }
    }
}

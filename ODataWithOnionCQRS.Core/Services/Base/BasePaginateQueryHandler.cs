using ODataWithOnionCQRS.Core.Query;
using MediatR;
using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Core.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.Entity;
using ODataWithOnionCQRS.Core.Extensions;

namespace ODataWithOnionCQRS.Services.Query
{
    public abstract class BasePaginateQueryHandler<TEntity, TDbContext> : IRequestHandler<PaginateQuery<TEntity>, PaginatedList<TEntity>>
        where TEntity : class
        where TDbContext : class, IDbContext
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public BasePaginateQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public PaginatedList<TEntity> Handle(PaginateQuery<TEntity> args)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                IDbContext dbCtx = dbContextScope.DbContexts.GetByInterface<TDbContext>();

                ((DbContext)dbCtx).Configuration.ProxyCreationEnabled = false;

                IQueryable<TEntity> entities = dbCtx.Set<TEntity>();

                entities = entities.Include(args);
                entities = entities.Where(args);
                entities = entities.OrderBy(args);
                var total = entities.Count();
                entities = entities.Paginate(args);

                return new PaginatedList<TEntity>(entities.ToList(), args.PageIndex, args.PageSize, total);
            }
        }
    }
}

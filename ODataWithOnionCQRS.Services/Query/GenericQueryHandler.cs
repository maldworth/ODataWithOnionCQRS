using ODataWithOnionCQRS.Core.Query;
using MediatR;
using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Core.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.Entity;
using ODataWithOnionCQRS.Core.Services.Base;

namespace ODataWithOnionCQRS.Services.Query
{
    public class GenericQueryHandler<TEntity> : BaseGenericQueryHandler<TEntity, ISchoolDbContext>
        where TEntity : class
    {
        public GenericQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
            : base(dbContextScopeFactory) { }
    }

    //public class GenericQueryHandler<TEntity> : IRequestHandler<GenericQuery<TEntity>, IEnumerable<TEntity>>
    //    where TEntity : class
    //{
    //    private readonly IDbContextScopeFactory _dbContextScopeFactory;

    //    public GenericQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
    //    {
    //        if (dbContextScopeFactory == null)
    //            throw new ArgumentNullException("dbContextScopeFactory");

    //        _dbContextScopeFactory = dbContextScopeFactory;
    //    }

    //    public IEnumerable<TEntity> Handle(GenericQuery<TEntity> args)
    //    {
    //        using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
    //        {
    //            var dbCtx = dbContextScope.DbContexts.GetByInterface<ISchoolDbContext>();
                
    //            ((DbContext)dbCtx).Configuration.ProxyCreationEnabled = false;

    //            IQueryable<TEntity> entities = dbCtx.Set<TEntity>();

    //            entities = entities.Include(args);
    //            entities = entities.Where(args);
    //            entities = entities.OrderBy(args);

    //            // Depending on your needs, you may not want to have .Take be mandatory
    //            return entities.Take(args.PageSize).ToList();
    //        }
    //    }
    //}
}

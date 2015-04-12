using ODataWithOnionCQRS.Core.Data;
using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Core.Query;
using MediatR;
using Mehdime.Entity;
using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;

namespace ODataWithOnionCQRS.Services.Query
{
    public class AutoMapperQueryHandler<TSrcEntity, TDestModel> : IRequestHandler<AutoMapperQuery<TSrcEntity, TDestModel>, IEnumerable<TDestModel>>
        where TSrcEntity : class
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public AutoMapperQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public IEnumerable<TDestModel> Handle(AutoMapperQuery<TSrcEntity, TDestModel> args)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                // Gets our context from our context scope
                var dbCtx = dbContextScope.DbContexts.GetByInterface<ISchoolDbContext>();

                IQueryable<TSrcEntity> srcEntities = dbCtx.Set<TSrcEntity>();

                srcEntities = srcEntities.Where(args);
                IQueryable<TDestModel> destEntities = srcEntities.Project().To<TDestModel>();
                destEntities = destEntities.OrderBy(args);
                return destEntities.Take(args.PageSize).ToList();
            }
        }
    }
}

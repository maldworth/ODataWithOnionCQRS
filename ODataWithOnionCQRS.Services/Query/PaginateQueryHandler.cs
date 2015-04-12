using ODataWithOnionCQRS.Core.Query;
using MediatR;
using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Core.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.Entity;

namespace ODataWithOnionCQRS.Services.Query
{
    public class PaginateQueryHandler<TEntity> : BasePaginateQueryHandler<TEntity, ISchoolDbContext>
        where TEntity : class
    {
        public PaginateQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
            : base(dbContextScopeFactory) { }
    }
}

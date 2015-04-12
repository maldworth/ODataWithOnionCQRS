using ODataWithOnionCQRS.Core.Query;
using MediatR;
using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Core.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using ODataWithOnionCQRS.Core.DomainModels;
using System.Data.Entity;

namespace ODataWithOnionCQRS.Services.Query
{
    public class BestMarkQueryHandler : IRequestHandler<BestMarkQuery, Enrollment>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public BestMarkQueryHandler(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public Enrollment Handle(BestMarkQuery query)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                var dbCtx = dbContextScope.DbContexts.GetByInterface<ISchoolDbContext>();

                ((DbContext)dbCtx).Configuration.ProxyCreationEnabled = false;

                var enrollment = dbCtx.Enrollments.Where(x => x.StudentId == query.StudentId && x.Grade != null).OrderBy(x => x.Grade).FirstOrDefault();
                if(enrollment == null)
                {
                    throw new InvalidOperationException("No enrollments found for this student");
                }

                return enrollment;
            }
        }
    }
}

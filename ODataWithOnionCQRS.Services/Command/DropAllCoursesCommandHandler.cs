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
using ODataWithOnionCQRS.Core.Command;

namespace ODataWithOnionCQRS.Services.Command
{
    public class DropAllCoursesCommandHandler : RequestHandler<DropAllCoursesCommand>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public DropAllCoursesCommandHandler(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        protected override void HandleCore(DropAllCoursesCommand command)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                // Gets our context from our context scope
                var dbCtx = dbContextScope.DbContexts.GetByInterface<ISchoolDbContext>();

                // Gets all enrollments that match student ID. Could have verified StudentID exists, but chose not to.
                var enrollments = dbCtx.Enrollments.Where(x => x.StudentId == command.StudentId).ToList();
                if(enrollments == null || enrollments.Count <= 0)
                {
                    throw new InvalidOperationException("No Enrollments found to drop.");
                }

                dbCtx.Enrollments.RemoveRange(enrollments);

                dbContextScope.SaveChanges();
            }
        }
    }
}

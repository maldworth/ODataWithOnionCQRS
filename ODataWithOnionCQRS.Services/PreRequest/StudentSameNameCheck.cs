using Mehdime.Entity;
using ODataWithOnionCQRS.Core.Command;
using ODataWithOnionCQRS.Core.Data;
using ODataWithOnionCQRS.Core.Services;
using ODataWithOnionCQRS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Services.PreRequest
{
    public class StudentSameNameCheck : IPreRequestHandler<CreateStudentCommand>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public StudentSameNameCheck(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
                throw new ArgumentNullException("dbContextScopeFactory");

            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public void Handle(CreateStudentCommand request)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                // Gets our context from our context scope
                var dbCtx = dbContextScope.DbContexts.GetByInterface<ISchoolDbContext>();

                // Find if Student with same firstmid and last name already exists
                var count = dbCtx.Students.Count(x => x.FirstMidName == request.FirstMidName && x.LastName == request.LastName);

                if (count > 0)
                    throw new InvalidOperationException("A Student with that last and firstmid name already exists");
            }
        }
    }
}

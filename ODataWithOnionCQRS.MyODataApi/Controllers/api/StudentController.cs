using MediatR;
using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.Core.Dto;
using ODataWithOnionCQRS.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ODataWithOnionCQRS.MyODataApi.Controllers.api
{
    public class StudentController : ApiController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        public IEnumerable<Student> Get()
        {
            // Example of calling the paginated result
            var pagedStudents = _mediator.Send(new PaginateQuery<Student>(2,3,orderBy: x=>x.OrderBy(c=>c.LastName)));

            // Depending on your design decisions, you might want to limit the amount of results that can be queries.
            // We made our services require a "Take" amount. You may not want to adopt this approach.
            var students = _mediator.Send(new GenericQuery<Student>(20));
            return students;
        }

        // I know this method is supposed to return the student's details, but I've just chosen to use this as an example of comparing the broad service AutoMapperQuery versus the specific BestMarkInCourse, and both achieve the same result albeit differently.
        public dynamic Get(int id)
        {
            // This query is almost identical to our BestMarkInCourse that we used in the odata function, but there's one difference. This result will return an ienumerable of 1 (by using the .Take(pageSize)), then calls FirstOrDefault
            // Versus, the BestMarkInCourseHandler, which actually calls the FirstOrDefault to trigger the DB Query.
            // So really, just two different ways to perform this query
            var bestMark = _mediator.Send(new AutoMapperQuery<Enrollment, BestMarkInCourseDto>(1, x => x.OrderBy(y => y.Grade), x => x.StudentId == id && x.Grade != null));
            return bestMark.FirstOrDefault();
        }

        //public void Post([FromBody]string value)
        //{
        //}

        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //public void Delete(int id)
        //{
        //}
    }
}

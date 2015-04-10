using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using ODataWithOnionCQRS.Core.DomainModels;
using MediatR;
using ODataWithOnionCQRS.Core.Data;
using ODataWithOnionCQRS.Core.Command;
using ODataWithOnionCQRS.Core.Query;
using ODataWithOnionCQRS.MyODataApi.Helpers;
using ODataWithOnionCQRS.MyODataApi.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ODataWithOnionCQRS.MyODataApi.Controllers
{
    public class StudentsController : ODataController
    {
        private readonly ISchoolDbContext _dbContext;
        private readonly IMediator _mediator;

        public StudentsController(ISchoolDbContext dbContext, IMediator mediator)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            _dbContext = dbContext;

            if (mediator == null)
                throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        #region Standard OData Endpoints (Mandatory)
        [EnableQuery]
        public IQueryable<Student> GetStudents()
        {
            return _dbContext.Students;
        }

        [EnableQuery]
        public SingleResult<Student> GetStudent([FromODataUri]int key)
        {
            return SingleResult.Create(_dbContext.Students.Where(m => m.Id == key));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Student> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentModel = await ODataBoilerplate.UpdateEntity(_dbContext, patch, key);

            return Updated(currentModel);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Student> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentModel = await ODataBoilerplate.UpdateEntity(_dbContext, patch, key);

            return Updated(currentModel);
        }


        public async Task<IHttpActionResult> Post(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await ODataBoilerplate.CreateEntity(_dbContext, student);

            return Created(student);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            await ODataBoilerplate.DeleteEntity<Student>(_dbContext, key);

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion

        // OData Actions
        // Are always HttpPost, and usually manipulate one or more entries in the Db.
        [HttpPost]
        public IHttpActionResult DropAllCourses([FromODataUri] int key)
        {
            // Void response from mediator
            _mediator.Send(new DropAllCoursesCommand { StudentId = key });
            return StatusCode(HttpStatusCode.NoContent);
        }

        // OData Functions
        // Are always HttpGet, and used for more custom queries, or projections
        [HttpGet]
        public IHttpActionResult BestMark([FromODataUri] int key)
        {
            // Actual Response from mediator
            var enrollment = _mediator.Send(new BestMarkQuery{ StudentId = key });
            return Ok(enrollment);
        }

        [HttpGet]
        public IHttpActionResult BestMarkInCourse([FromODataUri] int key)
        {
            // Example of response which uses automapper projections within the Handler
            var enrollmentDto = _mediator.Send(new BestMarkInCourseQuery { StudentId = key });
            return Ok(enrollmentDto);
        }

        [HttpGet]
        public IHttpActionResult CourseEnrollments()
        {
            // Example of response which uses automapper projections within the Handler
            var courseList = _mediator.Send(new AutoMapperQuery<Student, StudentCourseListViewModel>());
            return Ok(courseList);
        }
    }
}

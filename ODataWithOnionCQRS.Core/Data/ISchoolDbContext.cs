using ODataWithOnionCQRS.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Data
{
    public interface ISchoolDbContext : IDbContext
    {
        IMyAsyncDbSet<Student> Students { get; }
        IMyAsyncDbSet<Course> Courses { get; }
        IMyAsyncDbSet<Enrollment> Enrollments { get; }
    }
}

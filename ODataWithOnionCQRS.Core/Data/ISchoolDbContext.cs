using ODataWithOnionCQRS.Core.DomainModels;
using System.Data.Entity;

namespace ODataWithOnionCQRS.Core.Data
{
    public interface ISchoolDbContext : IDbContext
    {
        DbSet<Student> Students { get; }
        DbSet<Course> Courses { get; }
        DbSet<Enrollment> Enrollments { get; }
    }
}

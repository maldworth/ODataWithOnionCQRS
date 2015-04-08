using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.Core.Logging;
using Microsoft.AspNet.Identity.EntityFramework;
using ODataWithOnionCQRS.Core.Data;
using System.ComponentModel.DataAnnotations;
using ODataWithOnionCQRS.Data.Mappings;
using System;

namespace ODataWithOnionCQRS.Data
{
    public class SchoolDbContext : DbContext, ISchoolDbContext, IDbContextSaveChanges
    {
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;

        public SchoolDbContext()
            : base("name=SchoolContext") // use app.config transforms or web.config transforms to change this
        {
             if (_databaseInitialized)
             {
                 return;
             }
             lock (Lock)
             {
                 if (!_databaseInitialized)
                 {
                     // Set the database intializer which is run once during application start
                     // This seeds the database with admin user credentials and admin role
                     Database.SetInitializer(new ApplicationDbInitializer());
                     _databaseInitialized = true;
                 }
             }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new StudentMapping());
            modelBuilder.Configurations.Add(new EnrollmentMapping());
            modelBuilder.Configurations.Add(new CourseMapping());
        }


        // ISchoolDbContext implementation
        public IMyAsyncDbSet<Student> Students { get { return this.Set<Student>(); } }
        public IMyAsyncDbSet<Course> Courses { get { return this.Set<Course>(); } }
        public IMyAsyncDbSet<Enrollment> Enrollments { get { return this.Set<Enrollment>(); } }

        // IDbContext implementation
        public new IMyAsyncDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return new MyDbSet<TEntity>(base.Set<TEntity>());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.ViewModels
{
    public class CourseListViewModels
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public IEnumerable<CourseEnrollmentViewModel> Enrollments { get; set; }
    }

    public class CourseEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public string StudentLastName { get; set; }
    }
}

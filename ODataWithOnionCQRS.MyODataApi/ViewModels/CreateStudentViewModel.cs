using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataWithOnionCQRS.MyODataApi.ViewModels
{
    public class CreateStudentViewModel
    {
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
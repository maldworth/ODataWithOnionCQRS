using MediatR;
using ODataWithOnionCQRS.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Command
{
    public class CreateStudentCommand : IRequest<Student>
    {
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}

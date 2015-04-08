using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Command
{
    public class DropAllCoursesCommand : IRequest
    {
        public int StudentId { get; set; }
    }
}

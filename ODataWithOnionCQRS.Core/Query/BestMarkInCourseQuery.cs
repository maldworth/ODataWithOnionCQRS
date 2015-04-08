using ODataWithOnionCQRS.Core.Dto;
using MediatR;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.Core.Query
{
    public class BestMarkInCourseQuery : IRequest<BestMarkInCourseDto>
    {
        public int StudentId { get; set; }
    }
}

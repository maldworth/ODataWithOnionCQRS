using ODataWithOnionCQRS.Core.DomainModels;
using MediatR;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.Core.Query
{
    public class BestMarkQuery : IRequest<Enrollment>
    {
        public int StudentId { get; set; }
    }
}

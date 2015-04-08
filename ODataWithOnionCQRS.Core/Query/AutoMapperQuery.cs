using MediatR;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.Core.Query
{
    public class AutoMapperQuery<TSrcEntity, TDestModel> : IRequest<IEnumerable<TDestModel>>
    {
    }
}

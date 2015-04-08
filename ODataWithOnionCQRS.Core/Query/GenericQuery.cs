using MediatR;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.Core.Query
{
    public class GenericQuery<TEntity> : IRequest<IEnumerable<TEntity>>
    {
    }
}

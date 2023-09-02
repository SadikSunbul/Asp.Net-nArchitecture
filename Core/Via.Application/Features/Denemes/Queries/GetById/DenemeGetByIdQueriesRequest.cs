using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Via.Application.Features.Denemes.Queries.GetById
{
    public class DenemeGetByIdQueriesRequest:IRequest<DenemeGetByIdQueriesRespons>
    {
        public Guid Id { get; set; }

    }
}

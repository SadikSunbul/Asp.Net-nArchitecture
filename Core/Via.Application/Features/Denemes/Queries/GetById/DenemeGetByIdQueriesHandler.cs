using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Features.Denemes.Rules;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;

namespace Via.Application.Features.Denemes.Queries.GetById;

public class DenemeGetByIdQueriesHandler : IRequestHandler<DenemeGetByIdQueriesRequest, DenemeGetByIdQueriesRespons>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository denemeRepository;
    private readonly DenemeBusniessRules rules;

    public DenemeGetByIdQueriesHandler(IMapper mapper, IDenemeRepository denemeRepository, DenemeBusniessRules rules)
    {
        this.mapper = mapper;
        this.denemeRepository = denemeRepository;
        this.rules = rules;
    }

    public async Task<DenemeGetByIdQueriesRespons> Handle(DenemeGetByIdQueriesRequest request, CancellationToken cancellationToken)
    {
        Deneme deneme = await rules.IsThereSuchAnElementTo(request.Id);
        return mapper.Map<DenemeGetByIdQueriesRespons>(deneme);
    }
}

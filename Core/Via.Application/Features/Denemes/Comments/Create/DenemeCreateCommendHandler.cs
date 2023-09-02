using AutoMapper;
using MediatR;
using MediatR.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Features.Denemes.Rules;
using Via.Application.Services;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;

namespace Via.Application.Features.Denemes.Comments.Create;

public class DenemeCreateCommendHandler : IRequestHandler<DenemeCreateCommendRequest, DenemeCreateCommendRespons>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository repository;
    private readonly DenemeBusniessRules denemeBusniessRules;

    public DenemeCreateCommendHandler(IMapper mapper, IDenemeRepository repository, DenemeBusniessRules denemeBusniessRules)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.denemeBusniessRules = denemeBusniessRules;
    }

    public async Task<DenemeCreateCommendRespons> Handle(DenemeCreateCommendRequest request, CancellationToken cancellationToken)
    {
        await denemeBusniessRules.TrialNameUniquenessCheckAsync(request.Name);

        Deneme deneme = mapper.Map<Deneme>(request);

        if (deneme == null)
            return new() { Id = default, Name = null, Price = null };

        await repository.AddAsync(deneme);
        

        return mapper.Map<DenemeCreateCommendRespons>(deneme);
    }
}

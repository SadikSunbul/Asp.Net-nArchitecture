using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Features.Denemes.Rules;
using Via.Application.Services;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;

namespace Via.Application.Features.Denemes.Comments.Delete;

public class DenemeDeleteCommendHandler : IRequestHandler<DenemeDeleteCommendRequest, DenemeDeleteCommendRespons>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository repository;
    private readonly DenemeBusniessRules denemeBusniess;

    public DenemeDeleteCommendHandler(IMapper mapper, IDenemeRepository repository, DenemeBusniessRules denemeBusniess)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.denemeBusniess = denemeBusniess;
    }

    public async Task<DenemeDeleteCommendRespons> Handle(DenemeDeleteCommendRequest request, CancellationToken cancellationToken)
    {
        Deneme deneme = mapper.Map<Deneme>(request);

        if (deneme == null)
            return new DenemeDeleteCommendRespons() { Id = default, Name = null };

        var data = await denemeBusniess.IsThereSuchAnElementTo(deneme.Id);

        await repository.DeleteAsync(data);

        return mapper.Map<DenemeDeleteCommendRespons>(deneme);
    }
}

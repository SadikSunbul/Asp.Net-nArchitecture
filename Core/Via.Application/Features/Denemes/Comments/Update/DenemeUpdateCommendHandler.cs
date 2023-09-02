using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Features.Denemes.Constants;
using Via.Application.Features.Denemes.Rules;
using Via.Application.Services;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;

namespace Via.Application.Features.Denemes.Comments.Update;

public class DenemeUpdateCommendHandler : IRequestHandler<DenemeUpdateCommendRequest, DenemeUpdateCommendRespons>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository repository;
    private readonly DenemeBusniessRules busniessRules;

    public DenemeUpdateCommendHandler(IMapper mapper, IDenemeRepository repository, DenemeBusniessRules busniessRules)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.busniessRules = busniessRules;
    }

    public async Task<DenemeUpdateCommendRespons> Handle(DenemeUpdateCommendRequest request, CancellationToken cancellationToken)
    {
        Deneme deneme = await busniessRules.IsThereSuchAnElementTo(request.Id);

        if (!string.IsNullOrEmpty(request.Name))
            deneme.Name = request.Name;
        if(!string.IsNullOrEmpty(request.Price))
            deneme.Price = request.Price;

        await repository.UpdateAsync(deneme);

        return mapper.Map<DenemeUpdateCommendRespons>(deneme);
    }
}

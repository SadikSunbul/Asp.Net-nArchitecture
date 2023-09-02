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
using ViabelliWebProject.Packages.Core.Application.Respons;
using ViabelliWebProject.Packages.Core.Persistance.PageActions;

namespace Via.Application.Features.Denemes.Queries.GetList;

public class DenemeGetListQueriesHandler : IRequestHandler<DenemeGetListQueriesRequest, GetListRespons<DenemeGetListQueriesDTO>>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository repository;

    public DenemeGetListQueriesHandler(IMapper mapper, IDenemeRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }



    async Task<GetListRespons<DenemeGetListQueriesDTO>> IRequestHandler<DenemeGetListQueriesRequest, GetListRespons<DenemeGetListQueriesDTO>>.Handle(DenemeGetListQueriesRequest request, CancellationToken cancellationToken)
    {
        Paginate<Deneme> list = await repository.GetListAsync(index: request.PageRequest.PageIndex, size: request.PageRequest.PageSize, cancellationToken: cancellationToken);

        GetListRespons<DenemeGetListQueriesDTO> respons = mapper.Map<GetListRespons<DenemeGetListQueriesDTO>>(list);

        return respons;
    }
}

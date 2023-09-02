using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;
using ViabelliWebProject.Packages.Core.Application.Respons;
using ViabelliWebProject.Packages.Core.Persistance.PageActions;

namespace Via.Application.Features.Denemes.Queries.GetListByDynamic;

public class DenemeGetListByDynamicQueriesHandler : IRequestHandler<DenemeGetListByDynamicQueriesRequest, GetListRespons<DenemeGetListByDynamicQueriesDTO>>
{
    private readonly IMapper mapper;
    private readonly IDenemeRepository repository;

    public DenemeGetListByDynamicQueriesHandler(IMapper mapper, IDenemeRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }
    async Task<GetListRespons<DenemeGetListByDynamicQueriesDTO>> IRequestHandler<DenemeGetListByDynamicQueriesRequest, GetListRespons<DenemeGetListByDynamicQueriesDTO>>.Handle(DenemeGetListByDynamicQueriesRequest request, CancellationToken cancellationToken)
    {
        Paginate<Deneme> pageDeneme = await repository.GetListByDynamic(
          index: request.PageRequest.PageIndex,
          size: request.PageRequest.PageSize,
          dynamic: request.dynamicQuery);

        GetListRespons<DenemeGetListByDynamicQueriesDTO> respons = mapper.Map<GetListRespons<DenemeGetListByDynamicQueriesDTO>>(pageDeneme);

        return respons;
    }
}

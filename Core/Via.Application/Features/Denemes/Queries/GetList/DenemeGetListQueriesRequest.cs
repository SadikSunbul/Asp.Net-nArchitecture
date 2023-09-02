using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
using ViabelliWebProject.Packages.Core.Application.Request;
using ViabelliWebProject.Packages.Core.Application.Respons;

namespace Via.Application.Features.Denemes.Queries.GetList;

public class DenemeGetListQueriesRequest : IRequest<GetListRespons<DenemeGetListQueriesDTO>>, ICachebleRequest
{
    public PageRequest PageRequest { get; set; }

    public string CacheKey => $"DenemeGetListQueriesRequest({PageRequest.PageIndex},{PageRequest.PageSize})";

    public bool ByPassCache { get; }

    public TimeSpan? SlidiExpration { get; }

    public string? CacheGroupKey => "GetDeneme";
}

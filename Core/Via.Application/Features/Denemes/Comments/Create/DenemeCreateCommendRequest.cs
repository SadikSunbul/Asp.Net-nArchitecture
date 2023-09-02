using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
using ViabelliWebProject.Packages.Core.Application.Piplines.TransectionScopes;

namespace Via.Application.Features.Denemes.Comments.Create;

public class DenemeCreateCommendRequest : IRequest<DenemeCreateCommendRespons>,ICacheRemoverRequest
{
    public string Name { get; set; }
    public string Price { get; set; }

    public string? CacheKey => "";

    public bool BypassCache => false;

    public string? CacheGroupKey => "GetDeneme";
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Application.Request;
using ViabelliWebProject.Packages.Core.Application.Respons;
using ViabelliWebProject.Packages.Core.Persistance.Dynamic;

namespace Via.Application.Features.Denemes.Queries.GetListByDynamic;

public class DenemeGetListByDynamicQueriesRequest:IRequest<GetListRespons<DenemeGetListByDynamicQueriesDTO>>
{
    public DynamicQuery dynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }

}

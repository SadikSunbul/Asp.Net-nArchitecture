using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Features.Denemes.Comments.Create;
using Via.Application.Features.Denemes.Comments.Delete;
using Via.Application.Features.Denemes.Comments.Update;
using Via.Application.Features.Denemes.Queries.GetById;
using Via.Application.Features.Denemes.Queries.GetList;
using Via.Application.Features.Denemes.Queries.GetListByDynamic;
using Via.Domain.Entities;
using ViabelliWebProject.Packages.Core.Application.Respons;
using ViabelliWebProject.Packages.Core.Persistance.PageActions;

namespace Via.Application.Features.Denemes.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Deneme, DenemeCreateCommendRequest>().ReverseMap();
        CreateMap<Deneme, DenemeCreateCommendRespons>().ReverseMap();

        CreateMap<Deneme, DenemeDeleteCommendRequest>().ReverseMap();
        CreateMap<Deneme, DenemeDeleteCommendRespons>().ReverseMap();

        CreateMap<Deneme, DenemeUpdateCommendRequest>().ReverseMap();
        CreateMap<Deneme, DenemeUpdateCommendRespons>().ReverseMap();

        CreateMap<Deneme, DenemeGetListQueriesDTO>().ReverseMap();
        CreateMap<Paginate<Deneme>, DenemeGetListQueriesDTO>().ReverseMap();
        CreateMap<Paginate<Deneme>, GetListRespons<DenemeGetListQueriesDTO>>().ReverseMap();

        CreateMap<Deneme, DenemeGetByIdQueriesRespons>().ReverseMap();

        CreateMap<Deneme, DenemeGetListByDynamicQueriesDTO>().ReverseMap();
        CreateMap<Paginate<Deneme>, DenemeGetListByDynamicQueriesDTO>().ReverseMap();
        CreateMap<Paginate<Deneme>, GetListRespons<DenemeGetListByDynamicQueriesDTO>>().ReverseMap();


    }
}

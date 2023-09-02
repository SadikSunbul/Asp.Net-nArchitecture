using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Via.Application.Features.Denemes.Comments.Create;
using Via.Application.Features.Denemes.Comments.Delete;
using Via.Application.Features.Denemes.Comments.Update;
using Via.Application.Features.Denemes.Queries.GetById;
using Via.Application.Features.Denemes.Queries.GetList;
using Via.Application.Features.Denemes.Queries.GetListByDynamic;
using ViabelliWebProject.Packages.Core.Application.Request;

namespace Via.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DenemeController : ControllerBase
{
    private readonly IMediator mediator;

    public DenemeController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeneme([FromBody] DenemeCreateCommendRequest request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletedDeneme([FromBody] DenemeDeleteCommendRequest request)
    {
        var respons = await mediator.Send(request);
        return Ok(respons);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateDeneme([FromBody] DenemeUpdateCommendRequest request)
    {
        var respons = await mediator.Send(request);
        return Ok(respons);
    }

    [HttpGet]
    public async Task<IActionResult> GetListDeneme([FromQuery] PageRequest request)
    {
        var requestmediater = new DenemeGetListQueriesRequest() { PageRequest = request };

        var respons = await mediator.Send(requestmediater);

        return Ok(respons);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> GetByIdDenem([FromRoute] Guid id)
    {
        DenemeGetByIdQueriesRespons respons = await mediator.Send(new DenemeGetByIdQueriesRequest() { Id = id });
        return Ok(respons);
    }

    [HttpGet("DynamicList")]
    public async Task<IActionResult> DynamicLisDeneme([FromQuery] DenemeGetListByDynamicQueriesRequest request)
    {
        var respons = await mediator.Send(request);
        return Ok(respons);
    }
}
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Logging;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Logging;

public class LogginBehavior<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons> where TRequest : IRequest<TRespons>, ILoggableRequest
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly LoggerServiceBase loggerServiceBase;

    public LogginBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.loggerServiceBase = loggerServiceBase;
    }

    public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        List<LogParameter> logParameters = new()
        {
            new ()
            {
                Type=request.GetType().Name,
                Value=request,
                Name=next.Method.Name
            }
        };

        LogDetail logDetail = new LogDetail()
        {
            MethotName = next.Method.Name,
            Parameters = logParameters,
            User = httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"
        };

        loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
        return await next();
    }
}

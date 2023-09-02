using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ExceptionHandler;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Logging;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Midleware;
/// <summary>
/// Global hata yönetimini sağlıyan midleware sınıfı
/// </summary>
public class ExceptionMidleware
{
    /// <summary>
    /// Bir sonraki midleware yi çagırıcak field
    /// </summary>
    private readonly RequestDelegate _next;
    /// <summary>
    /// Gelen hataların filtrelenip hata mesajlarını üreticek olan sınıf
    /// </summary>
    private readonly HttpExceptionHandler exceptionHandler;

    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly LoggerServiceBase logger;

    public ExceptionMidleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, LoggerServiceBase logger)
    {
        _next = next;
        exceptionHandler = new HttpExceptionHandler();
        this.httpContextAccessor = httpContextAccessor;
        this.logger = logger;
    }
    //Default olrak br midlewarede olması gereken bir methot dur
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);//bir sonraki midlewareyi çalıştır
        }
        catch (Exception ex)
        {
            await LogException(context, ex);
            await ThereIsAnErrorAsync(context.Response, ex);//hata var ise git
        }
    }

    private Task LogException(HttpContext context, Exception ex)
    {
        List<LogParameter> logParameters = new List<LogParameter>()
        {
            new LogParameter()
            {
                Type=context.GetType().Name,
                Value=ex.ToString()
            }
        };

        LogDetailWithException logDetail = new LogDetailWithException()
        {
            MethotName = _next.Method.Name,
            Parameters = logParameters,
            User = httpContextAccessor.HttpContext.User.Identity?.Name ?? "?",
            ExcepsionMessage = ex.Message
        };

        logger.Error(JsonSerializer.Serialize(logDetail));
        return Task.CompletedTask;//ASENKRON DONME 
    }

    private Task ThereIsAnErrorAsync(HttpResponse response, Exception ex)
    {
        response.ContentType = "application/json"; //dönüceğimiz respons tipi
        exceptionHandler.Respons = response;
        return exceptionHandler.ExcepsionHandlerAsync(ex);
    }
}

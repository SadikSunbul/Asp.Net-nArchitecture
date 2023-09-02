using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Extensions;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ExceptionHandler;
/// <summary>
/// Hataları sınıflandırıp ilgili mesaj tiplerini üreten bir sınıftır 
/// </summary>
public class HttpExceptionHandler : ExceptionHandler
{
    /// <summary>
    /// Hatayı dönüceğimiz httprespons fieldı
    /// </summary>
    private HttpResponse response;
    /// <summary>
    /// Hatayı dönüceğimiz httprespons propertysi
    /// </summary>
    public HttpResponse Respons
    {
        get => response ?? throw new ArgumentNullException(nameof(response));
        set => response = value;
    }

    // Hata sınıflarının Şekillendirilme işlemleri 
    public override Task ExcepsionHandler(BusniesException busniesException)
    {
        Respons.StatusCode = StatusCodes.Status400BadRequest;
        string? message = new BusniesProblemDetails(busniesException.Message).AsJson();
        return Respons.WriteAsync(message);
    }

    public override Task ExcepsionHandler(Exception exception)
    {
        Respons.StatusCode = StatusCodes.Status500InternalServerError;
        string? message = new InternalServerProblemDetail(exception.Message).AsJson();
        return Respons.WriteAsync(message);
    }

    public override Task ExcepsionHandler(ValidationException validationException)
    {
        Respons.StatusCode = StatusCodes.Status400BadRequest;
        string? message = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Respons.WriteAsync(message);
    }

    public override Task ExcepsionHandler(TransectionalScopeException transectionalScopeException)
    {
        Respons.StatusCode = StatusCodes.Status400BadRequest;
        string? message = new TransectionalScopeProblemDeail(transectionalScopeException.Message).AsJson();
        return Respons.WriteAsync(message);
    }
}

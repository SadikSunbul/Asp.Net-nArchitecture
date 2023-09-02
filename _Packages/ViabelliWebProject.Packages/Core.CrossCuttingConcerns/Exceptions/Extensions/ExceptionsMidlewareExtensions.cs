using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Midleware;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Extensions;
/// <summary>
/// Global haata yönetiminin midleware sini IoC ye kaydını yapan sınıf
/// </summary>
public static class ExceptionsMidlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMidleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMidleware>();
}

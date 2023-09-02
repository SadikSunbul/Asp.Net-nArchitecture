using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Extensions;
/// <summary>
/// Problem Detail sınıfından türeyen sınıfların verilerini json formatına döndürmek için IoC ye kayıt ayapan sınıf
/// </summary>
public static class ProbemDetailExtensions
{/// <summary>
/// Problem Detail sınıfından türeyen sınıfların verilerini json formatına döndürmek için kullanılır
/// </summary>
    public static string AsJson<TProblemDetail>(this TProblemDetail detail) where TProblemDetail : ProblemDetails => JsonSerializer.Serialize(detail);
}

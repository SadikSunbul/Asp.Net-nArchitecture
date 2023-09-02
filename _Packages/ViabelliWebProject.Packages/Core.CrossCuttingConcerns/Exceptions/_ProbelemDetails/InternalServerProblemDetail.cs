using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails;
/// <summary>
/// Genel bilinmeyen hata türündeki hattaların bilgisini düzenlendiği (oluşturulduğu) yerdir
/// </summary>
public class InternalServerProblemDetail:ProblemDetails
{
    public InternalServerProblemDetail(string detail)
    {
        Title = "Internal server errors";
        Detail = detail;
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://viabelli.com/errors/internalserver";
    }
}

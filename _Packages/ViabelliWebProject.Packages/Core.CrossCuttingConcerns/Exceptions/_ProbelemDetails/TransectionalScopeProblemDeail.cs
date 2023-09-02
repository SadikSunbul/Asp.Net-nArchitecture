using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails;
/// <summary>
/// TransectionalScope hattaların bilgisini düzenlendiği (oluşturulduğu) yerdir
/// </summary>
public class TransectionalScopeProblemDeail : ProblemDetails
{
    public TransectionalScopeProblemDeail(string? message)
    {
        Title = "Transectional Scope errors";
        Detail = message;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://viabelli.com/error/transectional";
    }
}

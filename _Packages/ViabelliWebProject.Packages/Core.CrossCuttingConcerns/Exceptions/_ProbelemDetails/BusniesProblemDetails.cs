using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails;
/// <summary>
/// Busnies türündeki hattaların bilgisini düzenlendiği (oluşturulduğu) yerdir
/// </summary>
public class BusniesProblemDetails : ProblemDetails
{
    public BusniesProblemDetails(string detail)
    {
        Title = "Businies Error";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://viabelli.com/errors/businies";
    }
}

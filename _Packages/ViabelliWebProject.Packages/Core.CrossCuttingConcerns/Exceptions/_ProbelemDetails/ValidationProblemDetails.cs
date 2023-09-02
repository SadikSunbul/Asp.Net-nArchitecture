using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails;
/// <summary>
/// Validation türündeki hattaların bilgisini düzenlendiği (oluşturulduğu) yerdir
/// </summary>
public class ValidationProblemDetails: ProblemDetails
{
    public IEnumerable<ValidationExceptionModel> Errors { get; set; }
    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation Errors";
        Errors = errors;
        Detail= "One or more validation errors accurred";
        Status = StatusCodes.Status400BadRequest;
        Type = "https://viabelli.com/errors/validation";
    }
}

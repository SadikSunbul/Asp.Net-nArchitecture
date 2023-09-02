using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Logging;

public class LogDetailWithException : LogDetail
{
    public string ExcepsionMessage { get; set; }
    public LogDetailWithException()
    {
        ExcepsionMessage = string.Empty;
    }
    public LogDetailWithException(string fullName, string methotName, string user, List<LogParameter> parameters, string excepsionMessage) : base(fullName, methotName, user, parameters)
    {
        ExcepsionMessage = excepsionMessage;
    }
}

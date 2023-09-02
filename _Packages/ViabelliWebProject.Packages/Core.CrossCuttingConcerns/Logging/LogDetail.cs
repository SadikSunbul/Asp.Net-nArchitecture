using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Logging;

public class LogDetail
{
    public string FullName { get; set; }
    public string MethotName { get; set; }
    public string User { get; set; }
    public List<LogParameter> Parameters { get; set; }
    public LogDetail()
    {
        Parameters = new List<LogParameter>();
        FullName = string.Empty;
        MethotName = string.Empty;
        User = string.Empty;
    }
    public LogDetail(string fulName, string methotName, string user, List<LogParameter> logParameters)
    {
        FullName = fulName;
        MethotName = methotName;
        User = user;
        Parameters = logParameters;
    }
}

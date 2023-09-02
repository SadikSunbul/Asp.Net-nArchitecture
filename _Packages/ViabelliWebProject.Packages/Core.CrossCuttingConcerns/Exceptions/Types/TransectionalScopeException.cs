using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

public class TransectionalScopeException : Exception
{
    public TransectionalScopeException()
    {

    }
    public TransectionalScopeException(string? message) : base(message)
    {

    }
    public TransectionalScopeException(string? message, Exception exception) : base(message, exception)
    {

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

public class BusniesException : Exception
{
    public BusniesException()
    {

    }
    public BusniesException(string? message) : base(message)
    {

    }
    public BusniesException(string? message, Exception? exception) : base(message, exception)
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

public class ValidationException : Exception
{
    public IEnumerable<ValidationExceptionModel> Errors { get; set; }
    public ValidationException()
    {
        Errors = new List<ValidationExceptionModel>();
    }
    public ValidationException(string? message):base(message)
    {
        Errors = new List<ValidationExceptionModel>();
    }
    public ValidationException(string? message,Exception? exception):base(message, exception)
    {
        Errors=new List<ValidationExceptionModel>();
    }
    public ValidationException(IEnumerable<ValidationExceptionModel> errors):base(BuilErrorMessage(errors))
    {
        Errors = errors;
    }
    /// <summary>
    /// Gelen Hataları biçimlendirip stringe dödürür
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    private static string? BuilErrorMessage(IEnumerable<ValidationExceptionModel> errors)
    {
        IEnumerable<string> arr = errors.Select(
            x => $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors ?? Array.Empty<string>())}");
        return $"Validation faild: {string.Join(string.Empty, arr)}";
    }
}

/// <summary>
/// Validation hataları için bir model dir
/// </summary>
public class ValidationExceptionModel
{
    public string? Property { get; set; }
    public IEnumerable<string>? Errors { get; set; }

}

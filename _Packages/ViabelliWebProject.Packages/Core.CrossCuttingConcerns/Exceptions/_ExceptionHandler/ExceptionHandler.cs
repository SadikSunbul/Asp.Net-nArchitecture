using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ExceptionHandler;
/// <summary>
/// Hataların sınıflandırmasını modelleyen abstrac class
/// </summary>
public abstract class ExceptionHandler
{
    /// <summary>
    /// Gelen Hatayı Türüne göre filtreler ve ilgilihatanın mesajının üretilmesini sağlar
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public Task ExcepsionHandlerAsync(Exception exception) => exception switch
    {
        BusniesException busniesException => ExcepsionHandler(busniesException),
        ValidationException validationException=> ExcepsionHandler(validationException),
        TransectionalScopeException transectionalScopeException => ExcepsionHandler(transectionalScopeException),
        _ => ExcepsionHandler(exception)
    };

    public abstract Task ExcepsionHandler(BusniesException busniesException);
    public abstract Task ExcepsionHandler(ValidationException validationException);
    public abstract Task ExcepsionHandler(TransectionalScopeException transectionalScopeException);
    public abstract Task ExcepsionHandler(Exception exception);
}

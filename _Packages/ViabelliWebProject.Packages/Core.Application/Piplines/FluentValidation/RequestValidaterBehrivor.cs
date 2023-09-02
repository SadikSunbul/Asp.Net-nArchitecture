using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;
using ValidationException = ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.FluentValidation;
/// <summary>
/// Bu piplinenin çalışması için ilgili Requestin IRequest den türemesi gerekir. Bu pipline Request Geldiğinde tetiklenir gelen request türünde bir validate varmı yokmu onun kontrolunu yapar sartlara uyuyorsa sorunsuz calısıcaktır şartlara uymuyorsa hata fırlatıcaktır 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TRespons"></typeparam>
public class RequestValidaterBehrivor<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons>
    where TRequest : IRequest<TRespons>
{
    private readonly IEnumerable<IValidator<TRequest>> validater;//gelen requestin validaterlarını karsılar

    public RequestValidaterBehrivor(IEnumerable<IValidator<TRequest>> validater) //validater dan kalıtılanların burada yakalanması için IoC ye kaydının yapılması gerkir
    {
        this.validater = validater;
    }

    public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request); 

        IEnumerable<ValidationExceptionModel> errors = validater
            .Select(i => i.Validate(context)) //validate lerini getir
            .SelectMany(i => i.Errors) //errorlarını getir
            .Where(i => i != null) //eroru boş olmıyanları getir
            .GroupBy(keySelector: i => i.PropertyName,
            resultSelector: (propertyName, errors) => new ValidationExceptionModel { Property = propertyName, Errors = errors.Select(i => i.ErrorMessage) })//Hatranın oldugu properti ismini ve hata yı ilgili hata modeli ile modelle ve liste olarak dön
            .ToList();

        if(errors.Any()) //hata var ise hata fıralat
        {
            throw new ValidationException(errors);
        }
        TRespons respons = await next(); //hata yok sa devam et
        return respons;
    }
}

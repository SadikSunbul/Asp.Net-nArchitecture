using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.TransectionScopes;
/// <summary>
/// Çoklu işlemlerdeki hatalar sonucunda eksik veri tutulmasını engellemek için yazılmış bir pipline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TRespons"></typeparam>
public class TransectionScopeBehavior<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons> where TRequest : IRequest<TRespons>, ITransectionRequest
{
    public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled); //maliyetli oldugu için using kullandık burası calistiktan sornaki tüm işlemleri takip et dedik
        TRespons respons; 
        try
        {
            respons = await next(); //git işini yap gel 
            transactionScope.Complete(); //hata yok ise onayla işlemleri 
        }
        catch (Exception ex) //hata var ise 
        {
            transactionScope.Dispose(); //yapılan işlemeleri onaylama geriye al
            throw new TransectionalScopeException($"{request.GetType().Name} - {next.GetType().Name} - {next.Method.Name} =>Bu kısımdaki işlemlerden biri hatalı olduğu için tüm işlemler geri alınmıştır..."); //hata fırlat
        }
        return respons; 
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
/// <summary>
/// Bu pipline cache silme işlmeini yapıcaktır 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TRespons"></typeparam>
public class CacheRemoverRequestBehevior<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons> where TRequest : IRequest<TRespons>, ICacheRemoverRequest
{
    private readonly IDistributedCache cache; //.netin saglamıs oldugu cach yapısı

    public CacheRemoverRequestBehevior(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache) { return await next(); } //bypas edilmiş ise drekt geç bu işlemi der
        TRespons respons = await next(); //git sen önc bir veriyi olustur guncelle vb.. işini bi yap gel sen 

        if (request.CacheGroupKey != null)//requeste group key var ise gir
        {
            byte[]? cacheGroup = await cache.GetAsync(request.CacheGroupKey, cancellationToken); //group keydeki verileri getir

            if (cacheGroup != null) //veri var ise group ta gir
            {
                HashSet<string> keyInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroup));//gelen verielri listeye çevir
                foreach (var key in keyInGroup) //verielr içerisinde gez
                {
                    await cache.RemoveAsync(key, cancellationToken); //gelen baslıklardakı cacahlerı sil
                }

                await cache.RemoveAsync(request.CacheGroupKey, cancellationToken); //sonra group keyi de sil 
                await cache.RefreshAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken); //bunun zamanlayıcısınıda sil
            }
        }
        if (request.CacheKey != null) //eger cach key boş değilise gir
        {
            await cache.RemoveAsync(request.CacheKey, cancellationToken); //cach key e karsılık geln cach i sil
        }
        return respons;
    }
}

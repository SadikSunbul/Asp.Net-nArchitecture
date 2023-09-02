using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
/// <summary>
/// cachleme midlewaresi ICachebleRequest türündeki sinifları cachler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TRespons"></typeparam>
public class CachengBehavior<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons>
    where TRequest : IRequest<TRespons>, ICachebleRequest
{
    private readonly CacheSettings cacheSettings;//kendi sinifimız cach suresı için kullanıcaz
    private readonly IDistributedCache cache; //.net in saglamıs oldugu cach mekanizması

    public CachengBehavior(IDistributedCache cache, IConfiguration configuration)
    {//appsettingsten baglanıp ılgılı degerı alıyoruz 
        cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
        this.cache = cache;
    }

    public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        if (request.ByPassCache) { return await next(); } //cacah bypass ıse cachlemeye bakma der genelde bypassı geliştirme,test aşamasında kullanıcaz
        TRespons respons;
        byte[]? cacheRespons = await cache.GetAsync(request.CacheKey, cancellationToken);//cach keye göre git bak bakıyım böyle bir cach varmı yokmu var ise getir dedik

        if (cacheRespons != null)
        {//cach varise deserilize et ve geri dön
            respons = JsonSerializer.Deserialize<TRespons>(Encoding.UTF8.GetString(cacheRespons));
        }
        else
        {//cach yok ise git veri tabanından al o cachi sonra cach mekanizmasına kaydet
            respons = await getResponseAndAddToCache(request, next, cancellationToken);
        }
        return respons;
    }

    /// <summary>
    /// Veriatabından datayı al onu gönder ve cache ekle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<TRespons?> getResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
    {
        TRespons respons = await next(); //Datayı buradan alıyoruz 

        TimeSpan slidingExpiration = request.SlidiExpration ?? TimeSpan.FromDays(cacheSettings.SlidingExpiration); //zaman nekadar surelı bir cachleme olucak der eğer cache degerı girilmediyse appsetingsten default girilen değeri oku buraya ver der 

        DistributedCacheEntryOptions cacheOptions = new() //zamanı ekliyoruz
        {
            SlidingExpiration = slidingExpiration
        };
        byte[] serilizedata = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respons));//egenlen  datayı byte cevırıyoruz 
        await cache.SetAsync(request.CacheKey, serilizedata, cacheOptions, cancellationToken);//cache ekelem işlemi gerçekleşiyor

        if (request.CacheGroupKey != null) //Cachde group key var ise
            await addCacheKeyToGroup(request, slidingExpiration, cancellationToken);

        return respons;
    }
    /// <summary>
    /// Cachin Group keyi var ise git oraya su anki cach varmı yokmu kontrolunu yap yok ise şuanki cachin adını oraya yaz. Yazkı silme durumunda hepsi bir arada olsun
    /// </summary>
    /// <param name="request"></param>
    /// <param name="slidingExpiration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task addCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        byte[]? cacheGroupCache = await cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);//git group adındaki cach i al gel
        HashSet<string> cacheKeysInGroup; //verilere dışarıdan erişmek için dışarıda tanımladık 
        if (cacheGroupCache != null) //boş değil ise group içi gir
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;//içindeki verileri deserilize et
            if (!cacheKeysInGroup.Contains(request.CacheKey))//gelen veriler içerisinde şu anki cache key yok ise gir
                cacheKeysInGroup.Add(request.CacheKey); //şu anki cach key i ekle oraya 
        }
        else //eğer group içi boş ise giricek buraya 
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });//sadece şuanki cach key i ekle

        byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup); //veriler eklendikten sonra bunu byt e çevir

        //önbellek Grup Önbelleği Kayan Süre Sonu Önbelleği
        byte[]? cacheGroupCacheSlidingExpirationCache = await cache.GetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            cancellationToken); // bu kod parçası belirtilen anahtarla önbellekteki veriyi arar ve eğer bulunursa alır veriryi

        int? cacheGroupCacheSlidingExpirationValue = null;

        if (cacheGroupCacheSlidingExpirationCache != null) //yukarıda cachde aranan deger null degil ise
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));//o veriyi alır ve bu veriyi int e çevirerk atar ilgili yere

        if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue) //burada boyle bir cach yok ise yada zaman kucuk ıse girsin
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);//buradakı verıyı ınte cevir ve ver buradakı ıfade bize appsettingsten gelir

        byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue); //en sonda işlenmiş vereiyi byte çevir 

        DistributedCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        await cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken); //ilgili verielri kaydet


        await cache.SetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        ); //zamanlama cahi nide kaydet 
    }
}

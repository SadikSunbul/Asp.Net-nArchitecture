# ViabelliWebProject

## Repository
### Kullanım:
+ `ViabelliWebProject.Packages.Core.Persistance.Repositories` konumundaki `EFBaseRepository<TEntity, TEntityId, TContext>` içerisinde gelişmiş veri tabanı işlemleri içeirir.

+ İlk öncelikle ilgili Entitiy i `Entity<TId>` den kalım almalıdır .

+ Entitynin veri tabanı işlemlerini kazanması için `Via.Application.Services.Repositories` konumunda `I[isim]Repository` interfacesi oluşturulmalıdır. ve `IAsyncRepository` den kalıtılmalıdır
    + ```cs
        public interface I[isim]Repository : IAsyncRepository<Deneme, Guid>
        {
        }
         
+   Yukarıdaki işlemler yapıldıktan sonra `Via.Persistence.Repositories` konumuna `[isim]Repository` sınıfı olusturun
    + ```cs
         public class [isim]Repositroy : EFBaseRepository<Deneme, Guid, ViaBaseContext>, I[isim]Repository
         {
            public [isim]Repositroy(ViaBaseContext context) : base(context) > { }
         }

+ `Via.Persistence.PersistenceServicesRegisretions` konumuna altakini ekleyin
    + ```cs
             services.AddScoped<I[isim]Repository, [isim]Repositroy>();
+ İşlem tamamdır artık Yapmanız gereke nerede kullanmak isterseniz `I[isim]Repository` olarak IoC den talep etmektir



## Entity Freamework
+ `IDesignTimeDbContextFactory` içeriryor yanı .net cl ile migration vb. işlemelrde hata almazsınız
+ `appsetting.json` içerisine bağlantı adreslerinizi girebilirsiniz
    + ``` json
         "ConnectionStrings": {
         "mssqlSadıkDocker_Win": "Server=localhost, 1433;Database=denemeDbContext;User ID=SA;Password=şifre;TrustServerCertificate=True"
        }
    + Yukarıdaki dosyada bir isim değişikliği veyahutta yeni bir connection string oluşturduysanız ve onu kullanıcaksanız 2 yeri değiştirmelisiniz
        + 1-> `Via.Persistence.PersistenceServicesRegisretions` içindeki `AddDbContext` nin connection yerini değiştirin
        + 2->  `Via.Persistence.Context.DesingTimeDbContextFactory` içerisindeki `configurationManager.GetConnectionString("mssqlSadıkDocker_Win");` kısmını değiştiriniz


## Sayfalama Altyapısı
+ Verileri isteğinize göre sayfalanmasını kolaylaştıran bir alt yapıdır
+ Kullanım:
    +  `EFBaseRepository ` içerisinde  `GetListAsync` methodu bize sayfalama yapısını sunar
    +   Buradaki methodun parametrelerine bakarsanız sayfalama için bir `index` birde `size` ister
    +   Bu methodun geri dönüş değeri `Paginate<TEntity?>` dir
        + Bu methotun içerikleri
            + `Size` : Tek sayfa içerisindek item sayısını belirtir
            + `Index` : Sayfa indexini belirtir 1. sayfa 0. indextir
            + `Count` : Toplam item sayısını
            + `Page` : Toplam sayfa sayısı
            + `Items` : Elemanlar şarta uyan
            + `HasPrevius` : Bu sayfanın öncesi varmı
            + `HasNext` : Bu sayfanın sonrası varmı
        + Bunları kullana bilmek için 2 sınıfımız daha
            + `PageRequest` : sayfalandırma için gerekli özellikleri karsılar
            + `GetListRespons<TEntity>` : sayfalandırma sonucunda tüm sayfalandırma özellikelrini ve elemeanları karsılar

## Dynamic Query Altyapısı
+ Verileri isteğinize göre filtreleme ve sıralamanızı saglayan bir alt yapı sunar
+ `EFBaseRepository ` içerisinde  `GetListByDynamic` methodu bize (dynamic query + sayfalama) yapısını sunar
    +  `DynamicQuery` sınıfı bize sıralama ve dynamicquery yapmamızı sağlar içerisinde altakileri içerir
    + `IEnumerable<Sort>? Sort`
  >    + `Field` : ozellik neye gore bır sılralama olucak
  >    + `Dir` : desc , asc  hangı turde bır sıralama olucak
    + `Filter? Filter`
  >    + `Field` : Hangi özelliğe göre bir filtreleme yapılcak
  >    + `Value` : aranacak , sıralancak ,içeren vb. değer
  >    + `Operator` : ` "eq", "="` , `"neq", "!=" ` , ` "lt", "<"` , ` "lte", "<="` , `"gt", ">"` , `"gte", ">="` , ` "isnull", "== null" `, `"isnotnull", "!= null" ` ,`"startswith", "StartsWith"`,`"endswith", "EndsWith" `,`"contains", "Contains"`,`"doesnotcontain", "Contains"` bu operatörleri kullanabilirsiniz
  >    + `Logic` : `"and"` , ` "or"`
  >    + `IEnumerable<Filter>? Filters` :filtre içinde filtre gibi
    + Bu methot size Sayfalama yapısı sunucaktır sayfalama daki gibi kullanabilirsiniz


## AutoMapper yapısı
+ 2 sınıf arasındaki verileri Otomatik Eşleme işlemini yapar
+ Automapper kullanmak için ilgili sınıfı `Profile` den kalıtılması lazım ve constructer ına ilgili donusumler yazılmalıdır örnk:
    + ```cs
       
       public class MappingProfiles : Profile
       {
          public MappingProfiles()
          {
             CreateMap<Deneme, DenemeCreateCommendRequest>().ReverseMap();
             CreateMap<Deneme, DenemeCreateCommendRespons>().ReverseMap();
          }
        }
+Automapper IoC kaydı `Via.Application.ApplicationServicesRegisretions` konumundadır
+ ```cs
services.AddAutoMapper(Assembly.GetExecutingAssembly());

## MediatR patter
+  IoC kaydı `Via.Application.ApplicationServicesRegisretions` konumundadır
    +  ```cs
       services.AddMediatR(configure =>
           {
               configure.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
+ Herhangibir Actionda kullanılmak istenirse IoC den talep edilmelidir
    + ```cs
      [Route("api/[controller]")]
      [ApiController]
      public class DenemeController : ControllerBase
      {
          private readonly IMediator mediator;
      
          public DenemeController(IMediator mediator)
          {
              this.mediator = mediator;
          }
      }


## Rules
+ İlgili sınıf `ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Rules` konumundaki clastan kalıtılmalıdır aşağıdaki gibi işlerimizi burada yapıcaz
    + ```cs
     public class DenemeBusniessRules : BaseRules
     {
        private readonly IDenemeRepository denemeRepository;
    
        public DenemeBusniessRules(IDenemeRepository denemeRepository)
        {
            this.denemeRepository = denemeRepository;
        }
    
        public async Task TrialNameUniquenessCheckAsync(string name)
        {
            var data = await denemeRepository.AnyAsync(i => i.Name == name);
            if (data)
            {
                throw new BusniesException(DenemeMessage.DenemeNameExists);
            }
            return;
        }
      }
+ IoC ye ekleme otomatik bir şekilde yapılmasını sağlıycaz ` Via.Application.ApplicationServicesRegisretions` konumuda şunlar yapılmalıdır Dikakt edilirse iş sınıflarımızın hepsinini BaseRules den kalıtılması lazımdır değilse IoC ye ekliyemeyiz
    + ```cs
       public static class ApplicationServicesRegisretions
       {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
               services.AddSubClasesOfType(Assembly.GetExecutingAssembly(), typeof(BaseRules));
       
               return services;
            }
       
               public static IServiceCollection AddSubClasesOfType(
               this IServiceCollection services,
               Assembly assembly,
               Type type,
               Func<IServiceCollection, Type, IServiceCollection>? adWithLifeCycle = null)
            {
               var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
       
               foreach (var item in types)
               {
                   if (adWithLifeCycle == null)
                   {
                       services.AddScoped(item);
                   }
                   else
                   {
                       adWithLifeCycle(services, type);
                   }
               }
               return services;
            }
         }
+ IoC den talep Aşağıdaki şekildedir
    +  ```cs
        private readonly DenemeBusniessRules denemeBusniessRules;

        public DenemeCreateCommendHandler(DenemeBusniessRules denemeBusniessRules)
        {
           this.denemeBusniessRules = denemeBusniessRules;
        }

## Global Hata Yönetimi
+ İlk olarak Özel Hata sınıfınızı `ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types` konumunda oluşturunuz örnk:
    + ```cs
        public class BusniesException : Exception  //İhtiyaçlarınıza göre özelleştiriniz
        {
            public BusniesException()
            {
        
            }
        }
+ Sonra `ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ExceptionHandler` Konumuna geliniz
    + `ExceptionHandler` sınıfına giriniz ve aşşağıdaki gibi ilgili hatanızı ekleyiniz
        + ```cs
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
    + `HttpExceptionHandler` sınıfına giriniz ve aşşağıdaki gibi bir yapı kurgulayınız
        + ```cs
             public override Task ExcepsionHandler(BusniesException busniesException)
             {
                 Respons.StatusCode = StatusCodes.Status400BadRequest;
                 string? message = new BusniesProblemDetails(busniesException.Message).AsJson();
                 return Respons.WriteAsync(message);
             }
        + Üsteki `BusniesProblemDetails` sınıfını şu konuma oluşturunuz `ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions._ProbelemDetails` örnk:
            + ```cs
                 public class BusniesProblemDetails : ProblemDetails
                 {
                     public BusniesProblemDetails(string detail)
                     {
                         Title = "Businies Error";
                         Detail = detail;
                         Status = StatusCodes.Status400BadRequest;
                         Type = "https://viabelli.com/errors/businies";
                     }
                 }
    + Özelleştirilmiş hata işleminiz tamamlanmıştır ilgili Presentation katmanında Global Hatayı etkinleştirmek için şunu yazınız:
        + ```cs
             var app = builder.Build();
             // Configure the HTTP request pipeline.
             if (app.Environment.IsDevelopment())
             {
                 app.UseSwagger();
                 app.UseSwaggerUI();
             }
             //Global hata yönetimi 
             //if (app.Environment.IsProduction()) 
             //    app.UseGlobalExceptionMidleware();
             app.UseGlobalExceptionMidleware();

## Fluent Validation
+ İlgili sınıfın `AbstractValidator<T>` den kalıtılması lazımdır örnk:
    + ```cs
        public class DenemeCreateCommendValid : AbstractValidator<DenemeCreateCommendRequest>
        {
            public DenemeCreateCommendValid()
            {
                RuleFor(i => i.Name).NotEmpty().NotNull().MinimumLength(4);
            }
        }
    + Validation nun çalışması için `Via.Application.ApplicationServicesRegisretions` konumunu kontrol edin aşağıdaki kayıtların olaması gerekir (** ile işaretlendi)
        + ```cs
          public static IServiceCollection AddApplicationServices(this IServiceCollection services)
          {
              services.AddMediatR(configure =>
              {
                  configure.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
      
                  configure.AddOpenBehavior(typeof(RequestValidaterBehrivor<,>)); //**
              });
              services.AddAutoMapper(Assembly.GetExecutingAssembly()); //**
              return services;
          }

## TRansection
+ TRansection bize şunu sağlıycak diyelimki gelen request neticesinde 1 den fazla birbiri ile bağlantılı işlemler olduğnu düşünün örnk: Bir satış yapıldı ilgili ürünün ilk önce stok adeti düşürülür sonra ödeme alınır ödeme alınırken bir hata olduğu zaman tüm işlemlerin geri alınması lazımdır bu gibi durumlarda ilgili Requestinizi `ITransectionRequest` den kaltırsanız bu işlem otomatik bir şekilde gerçekleşicektir
    + ```cs
      public class DenemeCreateCommendRequest : IRequest<DenemeCreateCommendRespons>,ITransectionRequest
      {
          public string Name { get; set; }
          public string Price { get; set; }
      }
+ Transection nun çalışması için `Via.Application.ApplicationServicesRegisretions` Konumuna gidiniz ve şu yapılanmayı yapınız
+ ```cs
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configure.AddOpenBehavior(typeof(TransectionScopeBehavior<,>));
        });
## Cache mekanizması
+ Cache mekanizmasının alt yapısında `IDistributedCache` kullanıldı bu yüzden istediğiniz gibi şekkilendirebilir farklı teknolojilerle kullana bilirsiniz

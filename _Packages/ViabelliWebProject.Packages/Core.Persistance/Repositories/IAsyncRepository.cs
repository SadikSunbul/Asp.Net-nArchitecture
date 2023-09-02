using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Persistance.Dynamic;
using ViabelliWebProject.Packages.Core.Persistance.PageActions;

namespace ViabelliWebProject.Packages.Core.Persistance.Repositories;

public interface IAsyncRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
{
    /// <summary>
    /// Bu methot tek bir değer döner TEntity Türünden 
    /// predicate => while şartı sağlar 
    /// include => ilişkisel tablolar arası baglantıyı sağlar
    /// withDeleted => silinmiş değerleri getiriyimmi der
    /// enableTracking => Takip mekanizması çalışsınmı der
    /// cancellationToken => Durdurma yarıda kesme vb. işlemlerinde kullanılır 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="include"></param>
    /// <param name="withDeleted"></param>
    /// <param name="enableTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Bu yapı size sayfalama yapısı sunar 
    /// predicate => while şartı sağlar 
    /// include => ilişkisel tablolar arası baglantıyı sağlar
    /// withDeleted => silinmiş değerleri getiriyimmi der
    /// index => sayfa indexi kacıncı ındextekı sayfa olsun
    /// size => 1 sayfa içerisidneki eleman sayısı
    /// enableTracking => Takip mekanizması çalışsınmı der
    /// cancellationToken => Durdurma yarıda kesme vb. işlemlerinde kullanılır
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="incldue"></param>
    /// <param name="index"></param>
    /// <param name="size"></param>
    /// <param name="withDeleted"></param>
    /// <param name="enableTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paginate<TEntity?>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? incldue = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Bu methot bize filtereleme ve sayfalama işlevini aynı anda sunar
    /// dynamic => filterelem bilgilerini içerir
    /// predicate => while şartı sağlar 
    /// include => ilişkisel tablolar arası baglantıyı sağlar
    /// index => sayfa indexi kacıncı ındextekı sayfa olsun
    /// size => 1 sayfa içerisidneki eleman sayısı
    /// enableTracking => Takip mekanizması çalışsınmı der
    /// withDeleted => silinmiş değerleri getiriyimmi der
    /// cancellationToken => Durdurma yarıda kesme vb. işlemlerinde kullanılır
    /// </summary>
    /// <param name="dynamic"></param>
    /// <param name="predicate"></param>
    /// <param name="include"></param>
    /// <param name="index"></param>
    /// <param name="size"></param>
    /// <param name="withDeleted"></param>
    /// <param name="enableTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paginate<TEntity>> GetListByDynamic(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Şarta uygun bir değer içeriyormu içermiyormu diye kontrol eder ve bool döner
    /// predicate => while şartı sağlar 
    /// enableTracking => Takip mekanizması çalışsınmı der
    /// withDeleted => silinmiş değerleri getiriyimmi der
    /// cancellationToken => Durdurma yarıda kesme vb. işlemlerinde kullanılır
    /// </summary>
    /// <param name="predication"></param>
    /// <param name="withDeleted"></param>
    /// <param name="enableTracing"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predication = null,
        bool withDeleted = false,
        bool enableTracing = true,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Verilen tek bir elemanı ekler
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> AddAsync(TEntity entity);
    /// <summary>
    /// verilen eleman colleksiyonunu ekler
    /// </summary>
    /// <param name="entityes"></param>
    /// <returns></returns>
    /// 
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entityes);
    /// <summary>
    /// tek bır elemanın guncellemenı saglar
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity);
    /// <summary>
    /// eleman kolecsıyonunu toplu guncellemenı saglar
    /// </summary>
    /// <param name="entityes"></param>
    /// <returns></returns>
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entityes);
    /// <summary>
    /// silime işlmeini yapar şöyleki 2 türl üsilme işlemi yapar 1. kalıcı 2. işaretliyerek permanent = false geçici sil demek işaretle
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="permanent"></param>
    /// <returns></returns>
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);//permenent kalıcı demek kalıcı sılınsınmı 
    /// <summary>
    /// Kolleksiyon silime işlmeini yapar şöyleki 2 türl üsilme işlemi yapar 1. kalıcı 2. işaretliyerek permanent = false geçici sil demek işaretle
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="permanent"></param>
    /// <returns></returns>
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entityes, bool permanent = false);
}

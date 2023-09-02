using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.PageActions;

public static class IQueryablePaginateExtensions
{
    /// <summary>
    /// Sayfalama işlemini yapar 
    /// src : sayfalama yapılcak ham sorgu IQuerayable olmalıdır bu sorgu
    /// size : sayfadakı eleman sayısı
    /// index: sayfa indexi
    /// Geriye sayfa bilgileri ve sorgu netıcesınde gelen verileri verir
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="src"></param>
    /// <param name="size"></param>
    /// <param name="index"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Paginate<TEntity>> ToPaginateAsync<TEntity>(
        this IQueryable<TEntity> src,
        int size,
        int index,
        CancellationToken cancellationToken = default)
    {
        int count = await src.CountAsync(cancellationToken).ConfigureAwait(false);

        List<TEntity> items = await src
            .Skip(size * index)
            .Take(size)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        Paginate<TEntity> list = new Paginate<TEntity>()
        {
            Index = index,
            Size = size,
            Count = count,
            Items = items,
            Page = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }
}

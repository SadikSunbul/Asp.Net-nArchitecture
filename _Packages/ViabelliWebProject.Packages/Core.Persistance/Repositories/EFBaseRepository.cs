using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Persistance.Dynamic;
using ViabelliWebProject.Packages.Core.Persistance.PageActions;

namespace ViabelliWebProject.Packages.Core.Persistance.Repositories;
/// <summary>
/// Entity Framework için genel sorgu işlemlerini barındırır Asenkron olarak ypar bunları
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityId"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class EFBaseRepository<TEntity, TEntityId, TContext> :
    IAsyncRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TContext : DbContext
{
    protected readonly TContext context;

    public EFBaseRepository(TContext context)
    {
        this.context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreateDate = DateTime.UtcNow;
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entityes)
    {
        foreach (var item in entityes)
        {
            item.CreateDate = DateTime.UtcNow;
        }
        await context.Set<TEntity>().AddRangeAsync(entityes);
        await context.SaveChangesAsync();
        return entityes;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predication = null,
        bool withDeleted = false,
        bool enableTracing = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Query();
        if (predication != null)
            query = query.Where(predication);
        if (withDeleted)
            query = query.IgnoreQueryFilters();
        if (!enableTracing)
            query = query.AsNoTracking();
        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }


    #region Deleted
    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        await context.SaveChangesAsync();
        return entity;
    }

    private async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {//kalıcı değil ise
            CheckHasEntityHaveOneToOneRelation(entity); //1 e 1 ilişkisi varmı 
            await setEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            context.Remove(entity);
        }
    }

    private async Task setEntityAsSoftDeletedAsync(IEntityTimeStamps entity)
    {
        if (entity.DeletedDate.HasValue) //silinme degerı varmı yokmu ona bak varsa sılınmıs zaten
            return;
        entity.DeletedDate = DateTime.UtcNow;

        var navigations = context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is
            {
                IsOnDependent: false,
                ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
            })
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimeStamps navValueItem in (IEnumerable)navValue)
                    await setEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await setEntityAsSoftDeletedAsync((IEntityTimeStamps)navValue);
            }
        }

        context.Update(entity);
    }

    private IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((IEntityTimeStamps)x).DeletedDate.HasValue);
    }

    private void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation =
            context //ilgili contexte 
                .Entry(entity) //ilgili entitynin 
                .Metadata.GetForeignKeys() //meta datalarına bak forenkeylerini al
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true //koleksıyonmu 
                        || x.PrincipalToDependent?.IsCollection == true //koleksıyonmu 
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false; //1 e 1 varmı yokmu ona bak koleksıyonmu degılmı ona bakarak karar vere biliriz
        if (hasEntityHaveOneToOneRelation) //1 e 1 ise uyarı yapıyor
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key."
            );
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entityes, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entityes, permanent);
        await context.SaveChangesAsync();
        return entityes;
    }

    private async Task SetEntityAsDeletedAsync(ICollection<TEntity> entityes, bool permanent)
    {
        foreach (var entity in entityes)
        {
            await SetEntityAsDeletedAsync(entity, permanent);
        }
    }
    #endregion

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> quriyable = Query();
        if (withDeleted)
            quriyable = quriyable.IgnoreQueryFilters();
        if (!enableTracking)
            quriyable = quriyable.AsNoTracking();
        if (include != null)
            quriyable = include(quriyable);
        return await quriyable.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Paginate<TEntity?>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {

        IQueryable<TEntity> quriyable = Query();
        if (withDeleted)
            quriyable = quriyable.IgnoreQueryFilters();
        if (!enableTracking)
            quriyable = quriyable.AsNoTracking();
        if (include != null)
            quriyable = include(quriyable);
        if (orderBy != null)
            quriyable = orderBy(quriyable);
        if (predicate != null)
            quriyable = quriyable.Where(predicate);
        return await quriyable.ToPaginateAsync(index: index, size: size, cancellationToken: cancellationToken);/*.ConfigureAwait(false);*/
    }

    public async Task<Paginate<TEntity>> GetListByDynamic(
        DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index: index, size: size, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public IQueryable<TEntity> Query() => context.Set<TEntity>().AsQueryable();

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdateDate = DateTime.UtcNow;
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entityes)
    {
        foreach (var entity in entityes)
        {
            entity.UpdateDate = DateTime.UtcNow;
        }
        context.UpdateRange(entityes);
        await context.SaveChangesAsync();
        return entityes;
    }
}

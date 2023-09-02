using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.Repositories;
/// <summary>
/// Bu interface içerisinde IQueryable<TEntity> tütünden bir deger dönen bir methot içerir 
/// TEntity : int,string,Guid vb..
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IQuery<TEntity>
{
    /// <summary>
    /// Bu methodun kullanımım amacı bitmemiş sorgu olsturmak için kullanılır
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> Query();
}

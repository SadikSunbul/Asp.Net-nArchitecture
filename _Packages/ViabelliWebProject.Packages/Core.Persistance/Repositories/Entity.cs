using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.Repositories;

/// <summary>
/// Entitiyler için temel sınıfımızdır Entitylerin hepsini bu sınıftan kalıtılması şarttır 
/// Kalıtılmadığı takdirde bazı sistem özellklerini kullanamaya bilirsiniz
/// IEntityId : int,string,Guid  vb. şeklinde verilmelidir
/// </summary>
/// <typeparam name="IEntityId"></typeparam>
public class Entity<IEntityId>: IEntityTimeStamps
{
    public IEntityId Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Entity()
    {
        Id = default;
    }
    public Entity(IEntityId id)
    {
        Id = id;

    }
}

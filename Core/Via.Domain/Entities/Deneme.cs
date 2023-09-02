using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;

namespace Via.Domain.Entities;

public class Deneme:Entity<Guid>
{
    public string Name { get; set; }
    public string Price { get; set; }

}

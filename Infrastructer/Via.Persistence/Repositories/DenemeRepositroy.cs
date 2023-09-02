using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;

namespace Via.Persistence.Repositories;

public class DenemeRepositroy : EFBaseRepository<Deneme, Guid, ViaBaseContext>, IDenemeRepository
{
    public DenemeRepositroy(ViaBaseContext context) : base(context)
    {
    }
}




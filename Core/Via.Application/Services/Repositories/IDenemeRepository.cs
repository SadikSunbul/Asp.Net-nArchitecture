using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Domain.Entities;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;

namespace Via.Application.Services.Repositories;

public interface IDenemeRepository : IAsyncRepository<Deneme, Guid>
{
}

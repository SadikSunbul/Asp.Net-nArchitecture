using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.Repositories;

public class UserRepository : EFBaseRepository<User, int, ViaBaseContext>, IUserRepository
{
    public UserRepository(ViaBaseContext context)
        : base(context)
    {
    }
}
using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.Repositories;

public class OtpAuthenticatorRepository : EFBaseRepository<OtpAuthenticator, int, ViaBaseContext>,
    IOtpAuthenticatorRepository
{
    public OtpAuthenticatorRepository(ViaBaseContext context)
        : base(context)
    {
    }
}
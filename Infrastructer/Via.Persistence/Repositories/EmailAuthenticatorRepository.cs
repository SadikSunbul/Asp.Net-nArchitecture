using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.Repositories;

public class EmailAuthenticatorRepository : EFBaseRepository<EmailAuthenticator, int, ViaBaseContext>, IEmailAuthenticatorRepository
{
    public EmailAuthenticatorRepository(ViaBaseContext context)
        : base(context) { }
}


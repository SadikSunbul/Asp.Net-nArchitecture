using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Application.Services.Repositories;

public interface IEmailAuthenticatorRepository : IAsyncRepository<EmailAuthenticator, int>
{
}
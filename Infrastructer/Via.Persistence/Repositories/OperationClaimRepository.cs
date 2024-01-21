using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.Repositories;

public class OperationClaimRepository : EFBaseRepository<OperationClaim, int, ViaBaseContext>,
    IOperationClaimRepository
{
    public OperationClaimRepository(ViaBaseContext context)
        : base(context)
    {
    }
}
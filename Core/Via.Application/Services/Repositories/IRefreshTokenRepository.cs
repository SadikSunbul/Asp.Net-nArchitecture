using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Application.Services.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, int>
{
    Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userID, int refreshTokenTTL);
}
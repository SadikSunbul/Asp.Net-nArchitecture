using Microsoft.EntityFrameworkCore;
using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.Repositories;

public class RefreshTokenRepository : EFBaseRepository<RefreshToken, int, ViaBaseContext>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(ViaBaseContext context)
        : base(context)
    {
    }

    public async Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userID, int refreshTokenTTL)
    {
        List<RefreshToken> tokens = await Query()
            .AsNoTracking()
            .Where(
                r =>
                    r.UserId == userID
                    && r.Revoked == null
                    && r.Expires >= DateTime.UtcNow
                    && r.CreateDate.AddDays(refreshTokenTTL) <= DateTime.UtcNow
            )
            .ToListAsync();

        return tokens;
    }
}
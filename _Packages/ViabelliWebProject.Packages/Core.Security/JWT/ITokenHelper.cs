using ViabelliWebProject.Packages.Core.Security.Entities;

namespace ViabelliWebProject.Packages.Core.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, IList<OperationClaim> operationClaims); //token olustur

    RefreshToken CreateRefreshToken(User user, string ipAddress); //refresh token olustur
}
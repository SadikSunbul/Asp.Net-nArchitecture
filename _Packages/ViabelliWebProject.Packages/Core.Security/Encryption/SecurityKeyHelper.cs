using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ViabelliWebProject.Packages.Core.Security.Encryption;

public static class SecurityKeyHelper
{
    //bir güvenlik anahtarı oluşturmak için kullanılır
    public static SecurityKey CreateSecurityKey(string securityKey) =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    //using Microsoft.IdentityModel.Tokens;
    
}
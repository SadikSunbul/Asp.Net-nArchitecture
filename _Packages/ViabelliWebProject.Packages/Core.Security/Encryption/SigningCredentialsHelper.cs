using Microsoft.IdentityModel.Tokens;

namespace ViabelliWebProject.Packages.Core.Security.Encryption;

public static class SigningCredentialsHelper
{
    //bir güvenlik anahtarı ve bir imza algoritması kullanarak dijital imza oluşturmak için kullanılan bir sınıftır
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) =>
        new(securityKey, SecurityAlgorithms.HmacSha512Signature);

    /*
     *Bu metodun sonucunda elde edilen SigningCredentials nesnesi, genellikle bir JWT oluştururken kullanılarak,
     * token'in imzalanması için gerekli bilgileri içerir. Bu şekilde, güvenlik anahtarı ve imza algoritması
     * belirtilerek dijital imza işlemi gerçekleştirilebilir.
     */
}
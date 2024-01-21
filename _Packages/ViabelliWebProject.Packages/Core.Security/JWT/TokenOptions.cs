namespace ViabelliWebProject.Packages.Core.Security.JWT;

// Bu sınıf, JWT oluşturulurken kullanılacak belirli token seçeneklerini temsil eder.
public class TokenOptions
{
    // Token'ın hedef kitlesi (audience).
    public string Audience { get; set; }
    
    // Token'ın yayıncısı (issuer).
    public string Issuer { get; set; }
    
    // Erişim token'ının geçerlilik süresi (dakika cinsinden).
    public int AccessTokenExpiration { get; set; }
    
    // Güvenlik anahtarı.
    public string SecurityKey { get; set; }
    
    // Yenileme token'ının geçerlilik süresi (dakika cinsinden).
    public int RefreshTokenTTL { get; set; }

    public TokenOptions()
    {
        Audience = string.Empty;
        Issuer = string.Empty;
        SecurityKey = string.Empty;
    }

    public TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey, int refreshTokenTtl)
    {
        Audience = audience;
        Issuer = issuer;
        AccessTokenExpiration = accessTokenExpiration;
        SecurityKey = securityKey;
        RefreshTokenTTL = refreshTokenTtl;
    }
}
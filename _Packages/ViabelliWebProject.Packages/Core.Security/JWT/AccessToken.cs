namespace ViabelliWebProject.Packages.Core.Security.JWT;

public class AccessToken
{
    public string Token { get; set; } //token bılgısı
    public DateTime Expiration { get; set; } //son

    public AccessToken()
    {
        Token = string.Empty;
    }

    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}
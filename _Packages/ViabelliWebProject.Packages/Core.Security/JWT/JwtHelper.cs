using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ViabelliWebProject.Packages.Core.Security.Encryption;
using ViabelliWebProject.Packages.Core.Security.Entities;
using ViabelliWebProject.Packages.Core.Security.Extensions;

namespace ViabelliWebProject.Packages.Core.Security.JWT;

// Bu sınıf, JWT oluşturma ve yenileme işlemlerini gerçekleştirmek için tasarlanmış bir yardımcı sınıftır.
public class JwtHelper : ITokenHelper
{
    // Uygulama konfigürasyonunu içeren bir IConfiguration nesnesi.
    public IConfiguration Configuration { get; } //appsettingse ulasmak ıcın aldık bunu 

    // JWT için gerekli token seçeneklerini içeren bir TokenOptions nesnesi.
    private readonly TokenOptions _tokenOptions;

    // Erişim token'ının geçerlilik süresini tutan bir DateTime nesnesi.
    private DateTime _accessTokenExpiration;

    // Yapılandırıcı: Uygulama konfigürasyonu üzerinden TokenOptions nesnesini alır.
    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        const string configurationSection = "TokenOptions";
        _tokenOptions =
            Configuration.GetSection(configurationSection)
                .Get<TokenOptions>() //get gelmez nıse bınderı ekle kutuphaneden 
            ?? throw new NullReferenceException(
                $"\"{configurationSection}\" section cannot found in configuration.");
    }

    // Kullanıcıya ait bilgiler ve talepler üzerinden erişim token'ı oluşturur.
    public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
    {
        // Erişim token'ının geçerlilik süresini belirler.
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        // Güvenlik anahtarını oluşturur.
        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);

        // İmza bilgilerini oluşturur.
        SigningCredentials signingCredentials =
            SigningCredentialsHelper.CreateSigningCredentials(securityKey);

        // JWT token'ını oluşturur.
        JwtSecurityToken jwt =
            CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);

        // JWT token'ını string olarak formatlayarak AccessToken nesnesini döndürür.
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string? token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken { Token = token, Expiration = _accessTokenExpiration };
    }

    // Kullanıcıya ait bilgiler ve IP adresi üzerinden yenileme token'ı oluşturur.
    public RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        // Yenileme token'ını oluşturur.
        RefreshToken refreshToken =
            new()
            {
                UserId = user.Id,
                Token = RandomRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                CreateById = ipAddress
            };

        return refreshToken;
    }

    // JWT token'ını oluşturur.
    public JwtSecurityToken CreateJwtSecurityToken(
        TokenOptions tokenOptions,
        User user,
        SigningCredentials signingCredentials,
        IList<OperationClaim> operationClaims
    )
    {
        JwtSecurityToken jwt =
            new(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
        return jwt;
    }

    // Kullanıcıya ait bilgiler ve talepler üzerinden JWT taleplerini belirler.
    private IEnumerable<Claim> SetClaims(User user, IList<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();
        claims.AddNameIdentifier(user.Id.ToString()); //bunları bız yazmıstık extensıon olarak
        claims.AddEmail(user.Mail);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
        return claims; //claım lıstesıne cevırıp dondurme 
    }

    // Rastgele bir yenileme token'ı oluşturur.
    private string RandomRefreshToken()
    {
        byte[] numberByte = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}
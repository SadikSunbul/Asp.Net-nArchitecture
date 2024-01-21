using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ViabelliWebProject.Packages.Core.Security.Extensions;

// Bu statik sınıf, Claims (Talepler) üzerinde yaygın olarak kullanılan işlemleri gerçekleştirmek için genişletme metotları sağlar.

public static class ClaimExtensions
{
    // JWT'de kullanılacak bir email talebi ekler.
    public static void AddEmail(this ICollection<Claim> claims, string email) =>
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

    // JWT'de kullanılacak bir isim talebi ekler.
    public static void AddName(this ICollection<Claim> claims, string name) =>
        claims.Add(new Claim(ClaimTypes.Name, name));

    // JWT'de kullanılacak bir isim tanımlayıcı (identifier) talebi ekler.
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));

    // JWT'de kullanılacak rolleri ekler. Birden çok rol eklemek için dizi kullanır.
    public static void AddRoles(this ICollection<Claim> claims, string[] roles) =>
        roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
}
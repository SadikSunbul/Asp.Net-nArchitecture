using System.Security.Claims;

namespace ViabelliWebProject.Packages.Core.Security.Extensions;

// Bu statik sınıf, ClaimsPrincipal sınıfına ek işlevsellik sağlayan genişletme metotlarını içerir.
public static class ClaimsPrincipalExtensions
{
    // Belirli bir talep türüne sahip tüm talepleri listeleme.
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        // Verilen talep türüne sahip tüm talepleri bul ve değerlerini liste olarak döndür.
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    } //o ankı ksıının claımlerı 

    // Kullanıcının rollerini listeleme. (ClaimTypes.Role kullanılarak rol talepleri alınır.)
    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal?.Claims(ClaimTypes.Role); //o ankı kullanıcın claım rolelrı nelerdır

    // Kullanıcının kimlik numarasını alır. (ClaimTypes.NameIdentifier kullanılarak alınır.)
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());
}
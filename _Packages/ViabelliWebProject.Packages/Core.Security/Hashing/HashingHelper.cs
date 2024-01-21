using System.Security.Cryptography;
using System.Text;

namespace ViabelliWebProject.Packages.Core.Security.Hashing;

// Bu sınıf, kullanıcı şifrelerini güvenli bir şekilde hashleme ve doğrulama işlemlerini gerçekleştirmek için kullanılır.
public static class HashingHelper
{
    // Verilen şifre üzerinden bir şifre hash'i ve tuz oluşturur.
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();

        // Oluşturulan HMACSHA512'nin tuzunu alır.
        passwordSalt = hmac.Key;
        // Şifreyi UTF-8 formatında byte dizisine çevirip hash'ini alır.
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    // Verilen şifreyi, verilen hash ve tuz ile doğrular.
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        // Var olan tuz ile HMACSHA512 algoritması kullanarak yeni bir instance oluşturulur.
        using HMACSHA512 hmac = new(passwordSalt);

        // Girilen şifreyi UTF-8 formatında byte dizisine çevirip hash'ini alır.
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Hesaplanan hash ile verilen hash'i karşılaştırır ve sonucu döndürür.
        return computedHash.SequenceEqual(passwordHash);
    }
}
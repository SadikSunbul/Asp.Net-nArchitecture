using OtpNet;

namespace ViabelliWebProject.Packages.Core.Security.OtpAuthenticator.OtpNet;

// Bu sınıf, OtpNet kütüphanesini kullanarak OTP doğrulama işlemlerini gerçekleştirmek için tasarlanmış bir yardımcı sınıftır.
public class OtpNetOtpAuthenticatorHelper : IOtpAuthenticatorHelper
{
    // OTP için rastgele bir anahtar oluşturur.
    public Task<byte[]> GenerateSecretKey()
    {
        // Rastgele bir anahtar oluşturulur.
        byte[] key = KeyGeneration.GenerateRandomKey(20);

        // Anahtar, Base32Encoding kullanılarak string'e dönüştürülür.
        string base32String = Base32Encoding.ToString(key);
        // Base32 formatındaki string tekrar byte dizisine çevrilir.
        byte[] base32Bytes = Base32Encoding.ToBytes(base32String);

        return Task.FromResult(base32Bytes);
    }

    // Verilen byte dizisini Base32 formatındaki string'e dönüştürür.
    public Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        // Base32Encoding kullanılarak byte dizisi string'e dönüştürülür.
        string base32String = Base32Encoding.ToString(secretKey);
        return Task.FromResult(base32String);
    }

    // Verilen anahtar ve kullanıcı tarafından girilen kodu kullanarak OTP doğrulaması yapar.
    public Task<bool> VerifyCode(byte[] secretKey, string code)
    {
        // Verilen anahtardan Totp nesnesi oluşturulur.
        Totp totp = new(secretKey);

        // Geçerli zaman diliminde hesaplanan OTP kodu elde edilir.
        string totpCode = totp.ComputeTotp(DateTime.UtcNow);

        // Girilen kod ile hesaplanan kod karşılaştırılır ve sonuç döndürülür.
        bool result = totpCode == code;

        return Task.FromResult(result);
    }
}
using ViabelliWebProject.Packages.Core.Persistance.Repositories;

namespace ViabelliWebProject.Packages.Core.Security.Entities;

public class RefreshToken : Entity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; } //refres tokenın kendısı 
    public DateTime Expires { get; set; } //enzaman sonlanıcak
    public string CreateById { get; set; } //bunu olusturan ıp
    public DateTime? Revoked { get; set; } //iptal etme 
    public string? RevokedByIp { get; set; } //
    public string? ReplaceByToken { get; set; } //
    public string? ReasonRevoked { get; set; } //

    public virtual User User { get; set; } = null;

    public RefreshToken()
    {
        Token = string.Empty;
        CreateById = string.Empty;
    }

    public RefreshToken(int userId, string token, DateTime expires, string createById)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreateById = createById;
    }

    public RefreshToken(int id, int userId, string token, DateTime expires, string createById) : base(id)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreateById = createById;
    }
}
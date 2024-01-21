using ViabelliWebProject.Packages.Core.Persistance.Repositories;
using ViabelliWebProject.Packages.Core.Security.Enums;

namespace ViabelliWebProject.Packages.Core.Security.Entities;

public class User : Entity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mail { get; set; }
    public byte[] PasswordSalt { get; set; } //her kullanıcı için benzersiz ve rastgele bir değerdir.
    public byte[] PasswordHash { get; set; } //şifreyi gizler
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }


    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null;
    public virtual ICollection<OtpAuthenticator> OtpAuthenticators { get; set; } = null;
    public virtual ICollection<EmailAuthenticator> EmailAuthentictors { get; set; } = null;

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Mail = string.Empty;
        PasswordHash = Array.Empty<byte>();
        PasswordSalt = Array.Empty<byte>();
    }

    public User(string firstName, string lastName, string mail, byte[] passwordSalt, byte[] passwordHash,
        bool status)
    {
        FirstName = firstName;
        LastName = lastName;
        Mail = mail;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
    }

    public User(int id, string firstName, string lastName, string mail, byte[] passwordSalt,
        byte[] passwordHash, bool status) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Mail = mail;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
    }
}
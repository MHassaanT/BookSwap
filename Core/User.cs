using System;

namespace BookSwap.Core;

public class User
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";   // plain-text for now (txt-file storage)
    public string Role { get; set; } = "Student";    // "Student" | "Admin"
    public string ContactInfo { get; set; } = "";

    // ── serialisation helpers (CSV line: ID,Name,Email,PasswordHash,Role,ContactInfo) ──
    public override string ToString() =>
        $"{ID},{Name},{Email},{PasswordHash},{Role},{ContactInfo}";

    public static User? FromString(string line)
    {
        var p = line.Split(',');
        if (p.Length < 6) return null;
        return new User
        {
            ID           = Guid.TryParse(p[0], out var g) ? g : Guid.NewGuid(),
            Name         = p[1],
            Email        = p[2],
            PasswordHash = p[3],
            Role         = p[4],
            ContactInfo  = p[5]
        };
    }
}

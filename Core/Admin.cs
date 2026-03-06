namespace BookSwap.Core;

/// <summary>Admin is a User with Role == "Admin".
/// Kept as a thin subclass to satisfy the PDF OOP requirement.</summary>
public class Admin : User
{
    public Admin() => Role = "Admin";
}

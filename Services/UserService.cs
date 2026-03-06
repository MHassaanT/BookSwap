using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookSwap.Core;

namespace BookSwap.Services;

public static class UserService
{
    private static string DataFile => Path.Combine(AppContext.BaseDirectory, "users.txt");

    // ── Active session ──────────────────────────────────────────────
    public static User? CurrentUser { get; private set; }

    // ── Load ────────────────────────────────────────────────────────
    public static List<User> LoadUsers()
    {
        if (!File.Exists(DataFile)) return new();
        return File.ReadAllLines(DataFile)
                   .Where(l => !string.IsNullOrWhiteSpace(l))
                   .Select(User.FromString)
                   .Where(u => u != null)
                   .Cast<User>()
                   .ToList();
    }

    // ── Authenticate ────────────────────────────────────────────────
    public static User? Authenticate(string email, string password)
    {
        var user = LoadUsers().FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            u.PasswordHash == password);
        CurrentUser = user;
        return user;
    }

    // ── Register ────────────────────────────────────────────────────
    /// <returns>null on success, error message on failure</returns>
    public static string? Register(User user)
    {
        var users = LoadUsers();
        if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            return "An account with this email already exists.";

        user.ID = Guid.NewGuid();
        File.AppendAllText(DataFile, user.ToString() + Environment.NewLine);
        return null;
    }

    // ── Update Profile ──────────────────────────────────────────────
    public static void UpdateProfile(User updated)
    {
        var users = LoadUsers();
        var idx = users.FindIndex(u => u.ID == updated.ID);
        if (idx >= 0) users[idx] = updated;
        Save(users);
        CurrentUser = updated;
    }

    // ── Delete ──────────────────────────────────────────────────────
    public static void DeleteUser(Guid id)
    {
        var users = LoadUsers().Where(u => u.ID != id).ToList();
        Save(users);
    }

    private static void Save(List<User> users)
    {
        File.WriteAllLines(DataFile, users.Select(u => u.ToString()));
    }

    public static void Logout() => CurrentUser = null;
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BookSwap.Core;

namespace BookSwap.Services;

public static class BookService
{
    private static string BooksFile        => Path.Combine(AppContext.BaseDirectory, "books.json");
    private static string TransactionsFile => Path.Combine(AppContext.BaseDirectory, "transactions.json");

    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    // ── Books ────────────────────────────────────────────────────────
    public static List<Book> LoadBooks()
    {
        if (!File.Exists(BooksFile)) return SeedBooks();
        var json = File.ReadAllText(BooksFile);
        return JsonSerializer.Deserialize<List<Book>>(json, JsonOpts) ?? new();
    }

    public static void SaveBooks(List<Book> books)
    {
        File.WriteAllText(BooksFile, JsonSerializer.Serialize(books, JsonOpts));
    }

    public static void AddBook(Book book)
    {
        var books = LoadBooks();
        book.ID = books.Count > 0 ? books.Max(b => b.ID) + 1 : 1;
        books.Add(book);
        SaveBooks(books);
    }

    public static void UpdateBook(Book updated)
    {
        var books = LoadBooks();
        var idx = books.FindIndex(b => b.ID == updated.ID);
        if (idx >= 0) books[idx] = updated;
        SaveBooks(books);
    }

    public static void DeleteBook(int id)
    {
        var books = LoadBooks().Where(b => b.ID != id).ToList();
        SaveBooks(books);
    }

    // ── Transactions ─────────────────────────────────────────────────
    public static List<Transaction> LoadTransactions()
    {
        if (!File.Exists(TransactionsFile)) return new();
        var json = File.ReadAllText(TransactionsFile);
        return JsonSerializer.Deserialize<List<Transaction>>(json, JsonOpts) ?? new();
    }

    public static void SaveTransactions(List<Transaction> transactions)
    {
        File.WriteAllText(TransactionsFile, JsonSerializer.Serialize(transactions, JsonOpts));
    }

    public static void AddTransaction(Transaction t)
    {
        var transactions = LoadTransactions();
        transactions.Add(t);
        SaveTransactions(transactions);
    }

    // ── Purchase (atomic) ────────────────────────────────────────────
    /// Marks book as Sold and records the transaction in one call.
    public static void Purchase(Book book, Core.User buyer)
    {
        book.Status = "Sold";
        book.IsAvailable = false;
        UpdateBook(book);

        AddTransaction(new Transaction
        {
            BuyerID    = buyer.ID.ToString(),
            BuyerName  = buyer.Name,
            BookID     = book.ID,
            BookTitle  = book.Title,
            SellerName = book.Seller,
            FinalPrice = book.Price,
            Date       = DateTime.Now
        });
    }

    // ── Seed data ────────────────────────────────────────────────────
    private static List<Book> SeedBooks()
    {
        var books = new List<Book>
        {
            new() { ID = 1, Title = "Calc 101",          Author = "Newton",            Price = 45.00m, Category = "Engineering", Condition = "Good",      Status = "Available", Seller = "Alice",   IsAvailable = true  },
            new() { ID = 2, Title = "Circuit Analysis",  Author = "Tesla",             Price = 15.50m, Category = "Engineering", Condition = "Like New",  Status = "Available", Seller = "Bob",     IsAvailable = true  },
            new() { ID = 3, Title = "Modern Art History",Author = "Picasso",           Price = 30.00m, Category = "Arts",        Condition = "Fair",      Status = "Available", Seller = "Charlie", IsAvailable = true  },
            new() { ID = 4, Title = "Physics Vol 1",     Author = "Einstein",          Price = 19.99m, Category = "Science",     Condition = "Good",      Status = "Available", Seller = "Diana",   IsAvailable = true  },
            new() { ID = 5, Title = "C# Programming",    Author = "John",              Price = 29.99m, Category = "Science",     Condition = "Good",      Status = "Available", Seller = "Alice",   IsAvailable = true  },
            new() { ID = 6, Title = "Python Basics",     Author = "Jane",              Price = 24.99m, Category = "Science",     Condition = "Like New",  Status = "Available", Seller = "Bob",     IsAvailable = true  },
        };
        SaveBooks(books);
        return books;
    }
}

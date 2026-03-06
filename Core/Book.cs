using System.Text.Json.Serialization;

namespace BookSwap.Core;

public class Book
{
    public int    ID         { get; set; }
    public string Title      { get; set; } = "";
    public string Author     { get; set; } = "";
    public decimal Price     { get; set; }
    public string Condition  { get; set; } = "Good";
    public string Category   { get; set; } = "General";
    public string Status     { get; set; } = "Available";   // "Available" | "Sold"
    public string Seller     { get; set; } = "";
    public bool   IsAvailable { get; set; } = true;

    [JsonIgnore]
    public string DisplayPrice => $"${Price:F2}";
}

using System;

namespace BookSwap.Core;

public class Transaction
{
    public Guid     OrderID    { get; set; } = Guid.NewGuid();
    public string   BuyerID    { get; set; } = "";     // User.ID.ToString()
    public string   BuyerName  { get; set; } = "";
    public int      BookID     { get; set; }
    public string   BookTitle  { get; set; } = "";     // denormalised for display
    public string   SellerName { get; set; } = "";
    public decimal  FinalPrice { get; set; }
    public DateTime Date       { get; set; } = DateTime.Now;
}

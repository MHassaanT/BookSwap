namespace Module_2;

public class Book
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string Condition { get; set; } 
    public string Category { get; set; }  
    public string Status { get; set; }    
    
    public string DisplayPrice => $"${Price:F2}";
}
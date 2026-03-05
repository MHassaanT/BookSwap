#nullable disable  // Yeh sab warnings hata dega

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace BookSwapSimple;

// Book Class
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
    public string Seller { get; set; } = "";
    public bool IsAvailable { get; set; } = true;
}

// Bin Class
public class Bin
{
    private int id;
    private string name;
    private List<Book> bookList;

    public Bin(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.bookList = new List<Book>();
    }

    public void AddBook(Book book)
    {
        if (book == null)
        {
            Console.WriteLine("Book is null");
            return;
        }
        
        foreach (Book b in bookList)
        {
            if (b.Id == book.Id)
            {
                Console.WriteLine($"Book with ID {book.Id} already exists");
                return;
            }
        }
        
        bookList.Add(book);
        Console.WriteLine($"Book '{book.Title}' added");
    }

    public bool SearchBook(string title)
    {
        foreach (Book book in bookList)
        {
            if (book.Title == title)
            {
                return true;
            }
        }
        return false;
    }
    
    public List<Book> GetAllBooks()
    {
        return bookList;
    }
}

// Main Form
public class MainForm : Form
{
    // Controls
    private ListBox listBoxBooks;
    private TextBox txtTitle;
    private TextBox txtAuthor;
    private TextBox txtPrice;
    private Button btnAdd;
    private Button btnBuy;
    private Label lblStatus;
    
    // Data
    private Bin bookBin;
    private string dataFile = "books.json";

    public MainForm()
    {
        bookBin = new Bin(1, "Main Book Bin");
        
        this.Text = "Simple BookSwap";
        this.Size = new Size(600, 450);
        this.StartPosition = FormStartPosition.CenterScreen;

        CreateUI();  // Pehle UI banao
        LoadBooks(); // Phir books load karo
    }

    private void CreateUI()
    {
        // Title
        Label lblTitle = new Label();
        lblTitle.Text = "📚 BookSwap";
        lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
        lblTitle.Location = new Point(20, 15);
        lblTitle.Size = new Size(300, 40);

        // Book List - YAHAN INITIALIZE KARO
        listBoxBooks = new ListBox();
        listBoxBooks.Location = new Point(20, 60);
        listBoxBooks.Size = new Size(300, 250);

        // Add Book Section
        GroupBox groupAdd = new GroupBox();
        groupAdd.Text = "Add New Book";
        groupAdd.Location = new Point(340, 60);
        groupAdd.Size = new Size(220, 170);

        // Title field
        Label lblTitleLabel = new Label();
        lblTitleLabel.Text = "Title:";
        lblTitleLabel.Location = new Point(10, 25);
        lblTitleLabel.Size = new Size(40, 20);

        txtTitle = new TextBox();  // YAHAN INITIALIZE KARO
        txtTitle.Location = new Point(60, 23);
        txtTitle.Size = new Size(140, 20);

        // Author field
        Label lblAuthorLabel = new Label();
        lblAuthorLabel.Text = "Author:";
        lblAuthorLabel.Location = new Point(10, 55);
        lblAuthorLabel.Size = new Size(45, 20);

        txtAuthor = new TextBox();  // YAHAN INITIALIZE KARO
        txtAuthor.Location = new Point(60, 53);
        txtAuthor.Size = new Size(140, 20);

        // Price field
        Label lblPriceLabel = new Label();
        lblPriceLabel.Text = "Price:";
        lblPriceLabel.Location = new Point(10, 85);
        lblPriceLabel.Size = new Size(40, 20);

        txtPrice = new TextBox();  // YAHAN INITIALIZE KARO
        txtPrice.Location = new Point(60, 83);
        txtPrice.Size = new Size(140, 20);

        // Add button
        btnAdd = new Button();  // YAHAN INITIALIZE KARO
        btnAdd.Text = "Add Book";
        btnAdd.Location = new Point(10, 120);
        btnAdd.Size = new Size(190, 30);
        btnAdd.BackColor = Color.LightGreen;
        btnAdd.Click += BtnAdd_Click;

        groupAdd.Controls.Add(lblTitleLabel);
        groupAdd.Controls.Add(txtTitle);
        groupAdd.Controls.Add(lblAuthorLabel);
        groupAdd.Controls.Add(txtAuthor);
        groupAdd.Controls.Add(lblPriceLabel);
        groupAdd.Controls.Add(txtPrice);
        groupAdd.Controls.Add(btnAdd);

        // Buy button
        btnBuy = new Button();  // YAHAN INITIALIZE KARO
        btnBuy.Text = "Buy Selected Book";
        btnBuy.Location = new Point(20, 320);
        btnBuy.Size = new Size(150, 30);
        btnBuy.BackColor = Color.LightBlue;
        btnBuy.Click += BtnBuy_Click;

        // Status label
        lblStatus = new Label();  // YAHAN INITIALIZE KARO
        lblStatus.Location = new Point(20, 360);
        lblStatus.Size = new Size(400, 30);
        lblStatus.ForeColor = Color.Gray;

        // Sab controls form mein add karo
        this.Controls.Add(lblTitle);
        this.Controls.Add(listBoxBooks);
        this.Controls.Add(groupAdd);
        this.Controls.Add(btnBuy);
        this.Controls.Add(lblStatus);
    }

    private void RefreshList()
    {
        listBoxBooks.Items.Clear();
        var allBooks = bookBin.GetAllBooks();
        var availableBooks = allBooks.Where(b => b.IsAvailable).ToList();
        
        foreach (var book in availableBooks)
        {
            string displayText = $"{book.Title} - {book.Author} (${book.Price})";
            listBoxBooks.Items.Add(displayText);
        }
        
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        int available = bookBin.GetAllBooks().Count(b => b.IsAvailable);
        int total = bookBin.GetAllBooks().Count;
        lblStatus.Text = $"Available: {available}  |  Total: {total}";
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text) || 
            string.IsNullOrWhiteSpace(txtAuthor.Text) || 
            string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            MessageBox.Show("Please fill all fields!");
            return;
        }

        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            MessageBox.Show("Please enter a valid price!");
            return;
        }

        int newId = 1;
        var allBooks = bookBin.GetAllBooks();
        if (allBooks.Count > 0)
        {
            newId = allBooks.Max(b => b.Id) + 1;
        }

        Book newBook = new Book();
        newBook.Id = newId;
        newBook.Title = txtTitle.Text;
        newBook.Author = txtAuthor.Text;
        newBook.Price = price;
        newBook.Seller = "You";
        newBook.IsAvailable = true;

        bookBin.AddBook(newBook);
        
        SaveBooks();
        RefreshList();

        txtTitle.Text = "";
        txtAuthor.Text = "";
        txtPrice.Text = "";
        
        MessageBox.Show("Book added!");
    }

    private void BtnBuy_Click(object sender, EventArgs e)
    {
        if (listBoxBooks.SelectedItem == null)
        {
            MessageBox.Show("Please select a book!");
            return;
        }

        string selectedText = listBoxBooks.SelectedItem.ToString() ?? "";
        
        Book bookToBuy = null;
        foreach (var book in bookBin.GetAllBooks())
        {
            string displayText = $"{book.Title} - {book.Author} (${book.Price})";
            if (displayText == selectedText && book.IsAvailable)
            {
                bookToBuy = book;
                break;
            }
        }

        if (bookToBuy == null)
        {
            MessageBox.Show("Book not found!");
            return;
        }

        DialogResult result = MessageBox.Show(
            $"Buy '{bookToBuy.Title}' for ${bookToBuy.Price}?",
            "Confirm",
            MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
            bookToBuy.IsAvailable = false;
            SaveBooks();
            RefreshList();
            MessageBox.Show("Purchased!");
        }
    }

    private void LoadBooks()
    {
        if (File.Exists(dataFile))
        {
            string json = File.ReadAllText(dataFile);
            var books = JsonSerializer.Deserialize<List<Book>>(json);
            if (books != null)
            {
                foreach (var book in books)
                {
                    bookBin.AddBook(book);
                }
            }
        }
        else
        {
            // Sample books
            bookBin.AddBook(new Book { Id = 1, Title = "C# Programming", Author = "John", Price = 29.99m, Seller = "Alice", IsAvailable = true });
            bookBin.AddBook(new Book { Id = 2, Title = "Python Basics", Author = "Jane", Price = 24.99m, Seller = "Bob", IsAvailable = true });
            bookBin.AddBook(new Book { Id = 3, Title = "Java Guide", Author = "Mike", Price = 34.99m, Seller = "Charlie", IsAvailable = true });
            
            SaveBooks();
        }
        
        RefreshList();
    }

    private void SaveBooks()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(bookBin.GetAllBooks(), options);
        File.WriteAllText(dataFile, json);
    }
}

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
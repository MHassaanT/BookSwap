#nullable disable

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

// Transaction Class
public class Transaction
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = "";
    public string BuyerName { get; set; } = "";
    public string SellerName { get; set; } = "";
    public decimal Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}

// Main Form
public class MainForm : Form
{
    // Controls
    private ListBox listBoxBooks;
    private ListBox listBoxTransactions;
    private TextBox txtTitle;
    private TextBox txtAuthor;
    private TextBox txtPrice;
    private Button btnAdd;
    private Button btnBuy;
    private Label lblStatus;
    private TabControl tabControl;
    
    // Data
    private List<Book> books = new List<Book>();
    private List<Transaction> transactions = new List<Transaction>();
    private string booksFile = "books.json";
    private string transactionsFile = "transactions.json";

    public MainForm()
    {
        this.Text = "BookSwap - Module 3 with Transactions";
        this.Size = new Size(750, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        LoadData();
        CreateUI();
    }

    private void CreateUI()
    {
        // Title
        Label lblTitle = new Label();
        lblTitle.Text = "📚 BookSwap - Module 3 (Complete)";
        lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
        lblTitle.Location = new Point(20, 15);
        lblTitle.Size = new Size(500, 30);

        // Tab Control
        tabControl = new TabControl();
        tabControl.Location = new Point(20, 50);
        tabControl.Size = new Size(700, 480);

        // ========== TAB 1: MARKETPLACE ==========
        TabPage tabMarketplace = new TabPage("📖 Marketplace");
        
        // Book List
        Label lblAvailable = new Label();
        lblAvailable.Text = "Available Books:";
        lblAvailable.Font = new Font("Arial", 10, FontStyle.Bold);
        lblAvailable.Location = new Point(10, 10);
        lblAvailable.Size = new Size(150, 20);
        
        listBoxBooks = new ListBox();
        listBoxBooks.Location = new Point(10, 35);
        listBoxBooks.Size = new Size(350, 250);
        
        // Buy Button
        btnBuy = new Button();
        btnBuy.Text = "💰 Buy Selected Book";
        btnBuy.Location = new Point(10, 295);
        btnBuy.Size = new Size(200, 35);
        btnBuy.BackColor = Color.LightBlue;
        btnBuy.Font = new Font("Arial", 10, FontStyle.Bold);
        btnBuy.Click += BtnBuy_Click;
        
        // Add Book Section
        GroupBox groupAdd = new GroupBox();
        groupAdd.Text = "➕ Add New Book";
        groupAdd.Font = new Font("Arial", 10, FontStyle.Bold);
        groupAdd.Location = new Point(380, 20);
        groupAdd.Size = new Size(290, 250);

        Label lblTitleLabel = new Label() { Text = "Title:", Location = new Point(10, 30), Size = new Size(50, 20) };
        txtTitle = new TextBox() { Location = new Point(70, 28), Size = new Size(200, 20) };

        Label lblAuthorLabel = new Label() { Text = "Author:", Location = new Point(10, 65), Size = new Size(50, 20) };
        txtAuthor = new TextBox() { Location = new Point(70, 63), Size = new Size(200, 20) };

        Label lblPriceLabel = new Label() { Text = "Price:", Location = new Point(10, 100), Size = new Size(50, 20) };
        txtPrice = new TextBox() { Location = new Point(70, 98), Size = new Size(200, 20) };

        btnAdd = new Button();
        btnAdd.Text = "✅ Add Book";
        btnAdd.Location = new Point(10, 140);
        btnAdd.Size = new Size(260, 35);
        btnAdd.BackColor = Color.LightGreen;
        btnAdd.Font = new Font("Arial", 10, FontStyle.Bold);
        btnAdd.Click += BtnAdd_Click;

        groupAdd.Controls.Add(lblTitleLabel);
        groupAdd.Controls.Add(txtTitle);
        groupAdd.Controls.Add(lblAuthorLabel);
        groupAdd.Controls.Add(txtAuthor);
        groupAdd.Controls.Add(lblPriceLabel);
        groupAdd.Controls.Add(txtPrice);
        groupAdd.Controls.Add(btnAdd);
        
        tabMarketplace.Controls.Add(lblAvailable);
        tabMarketplace.Controls.Add(listBoxBooks);
        tabMarketplace.Controls.Add(btnBuy);
        tabMarketplace.Controls.Add(groupAdd);

        // ========== TAB 2: MY LISTINGS ==========
        TabPage tabListings = new TabPage("📋 My Listings");
        Label lblListings = new Label();
        lblListings.Text = "Your Listed Books (Coming Soon)";
        lblListings.Location = new Point(20, 20);
        lblListings.Size = new Size(300, 30);
        tabListings.Controls.Add(lblListings);

        // ========== TAB 3: TRANSACTIONS ==========
        TabPage tabTransactions = new TabPage("📦 Transactions");
        
        Label lblTransHeading = new Label();
        lblTransHeading.Text = "Purchase & Sale History:";
        lblTransHeading.Font = new Font("Arial", 11, FontStyle.Bold);
        lblTransHeading.Location = new Point(10, 10);
        lblTransHeading.Size = new Size(250, 25);
        
        listBoxTransactions = new ListBox();
        listBoxTransactions.Location = new Point(10, 40);
        listBoxTransactions.Size = new Size(660, 350);
        listBoxTransactions.Font = new Font("Consolas", 10);
        
        Button btnRefresh = new Button();
        btnRefresh.Text = "🔄 Refresh";
        btnRefresh.Location = new Point(10, 400);
        btnRefresh.Size = new Size(100, 30);
        btnRefresh.Click += (s, e) => RefreshTransactionsList();
        
        tabTransactions.Controls.Add(lblTransHeading);
        tabTransactions.Controls.Add(listBoxTransactions);
        tabTransactions.Controls.Add(btnRefresh);

        // ========== TAB 4: MY ORDERS ==========
        TabPage tabOrders = new TabPage("🛒 My Orders");
        Label lblOrders = new Label();
        lblOrders.Text = "Your Orders (Coming Soon)";
        lblOrders.Location = new Point(20, 20);
        lblOrders.Size = new Size(300, 30);
        tabOrders.Controls.Add(lblOrders);

        // Add tabs
        tabControl.TabPages.Add(tabMarketplace);
        tabControl.TabPages.Add(tabListings);
        tabControl.TabPages.Add(tabTransactions);
        tabControl.TabPages.Add(tabOrders);

        // Status
        lblStatus = new Label();
        lblStatus.Location = new Point(20, 540);
        lblStatus.Size = new Size(700, 20);
        lblStatus.ForeColor = Color.Gray;
        lblStatus.Font = new Font("Arial", 9);

        // Add to form
        this.Controls.Add(lblTitle);
        this.Controls.Add(tabControl);
        this.Controls.Add(lblStatus);

        RefreshBooksList();
        RefreshTransactionsList();
    }

    private void RefreshBooksList()
    {
        listBoxBooks.Items.Clear();
        var availableBooks = books.Where(b => b.IsAvailable).ToList();
        
        if (availableBooks.Count == 0)
        {
            listBoxBooks.Items.Add("⚠️ No books available. Add some books!");
        }
        else
        {
            foreach (var book in availableBooks)
            {
                listBoxBooks.Items.Add($"📘 {book.Title} by {book.Author} - ${book.Price:F2} [Seller: {book.Seller}]");
            }
        }
        
        UpdateStatus();
    }

    private void RefreshTransactionsList()
    {
        listBoxTransactions.Items.Clear();
        
        if (transactions.Count == 0)
        {
            listBoxTransactions.Items.Add("══════════════════════════════════════════════════");
            listBoxTransactions.Items.Add("        No transactions yet. Buy a book!        ");
            listBoxTransactions.Items.Add("══════════════════════════════════════════════════");
        }
        else
        {
            listBoxTransactions.Items.Add("┌─────────────────────────────────────────────────────────────────┐");
            listBoxTransactions.Items.Add("│                    TRANSACTION HISTORY                           │");
            listBoxTransactions.Items.Add("├─────────────────────────────────────────────────────────────────┤");
            
            foreach (var t in transactions.OrderByDescending(t => t.Date))
            {
                listBoxTransactions.Items.Add($"│ ID: {t.Id,-3} | {t.Date:yyyy-MM-dd HH:mm}                          │");
                listBoxTransactions.Items.Add($"│ Book: {t.BookTitle}                                             │");
                listBoxTransactions.Items.Add($"│ From: {t.SellerName,-15} To: {t.BuyerName,-15} Price: ${t.Price,-5}   │");
                listBoxTransactions.Items.Add($"├─────────────────────────────────────────────────────────────────┤");
            }
            listBoxTransactions.Items.Add("└─────────────────────────────────────────────────────────────────┘");
        }
    }

    private void UpdateStatus()
    {
        int available = books.Count(b => b.IsAvailable);
        int sold = books.Count - available;
        lblStatus.Text = $"📊 Available: {available} | Sold: {sold} | Total Transactions: {transactions.Count} | Total Books: {books.Count}";
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text) || 
            string.IsNullOrWhiteSpace(txtAuthor.Text) || 
            string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            MessageBox.Show("❌ Please fill all fields!", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            MessageBox.Show("❌ Please enter a valid price!", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Book newBook = new Book();
        newBook.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
        newBook.Title = txtTitle.Text;
        newBook.Author = txtAuthor.Text;
        newBook.Price = price;
        newBook.Seller = "You (User)";
        newBook.IsAvailable = true;

        books.Add(newBook);
        
        SaveData();
        RefreshBooksList();

        txtTitle.Text = "";
        txtAuthor.Text = "";
        txtPrice.Text = "";
        
        MessageBox.Show($"✅ Book '{newBook.Title}' added successfully!", "Success", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnBuy_Click(object sender, EventArgs e)
    {
        if (listBoxBooks.SelectedItem == null)
        {
            MessageBox.Show("❌ Please select a book to buy!", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (listBoxBooks.SelectedItem.ToString().Contains("⚠️ No books available"))
        {
            MessageBox.Show("❌ No books available to buy!", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string selectedText = listBoxBooks.SelectedItem.ToString();
        
        Book bookToBuy = null;
        foreach (var book in books)
        {
            if (selectedText.Contains(book.Title) && selectedText.Contains(book.Author) && book.IsAvailable)
            {
                bookToBuy = book;
                break;
            }
        }

        if (bookToBuy == null)
        {
            MessageBox.Show("❌ Book not found or already sold!", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        DialogResult result = MessageBox.Show(
            $"💰 Buy '{bookToBuy.Title}' for ${bookToBuy.Price:F2} from {bookToBuy.Seller}?",
            "Confirm Purchase",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // Book ko sold karo
            bookToBuy.IsAvailable = false;
            
            // Transaction create karo
            Transaction newTransaction = new Transaction();
            newTransaction.Id = transactions.Count > 0 ? transactions.Max(t => t.Id) + 1 : 1;
            newTransaction.BookId = bookToBuy.Id;
            newTransaction.BookTitle = bookToBuy.Title;
            newTransaction.BuyerName = "You (Buyer)";
            newTransaction.SellerName = bookToBuy.Seller;
            newTransaction.Price = bookToBuy.Price;
            newTransaction.Date = DateTime.Now;
            
            transactions.Add(newTransaction);
            
            SaveData();
            RefreshBooksList();
            RefreshTransactionsList();
            
            MessageBox.Show(
                $"✅ Purchase successful!\n\nTransaction ID: {newTransaction.Id}\nBook: {bookToBuy.Title}\nPrice: ${bookToBuy.Price:F2}\nDate: {newTransaction.Date:yyyy-MM-dd HH:mm}",
                "Purchase Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            
            // Transactions tab dikhao
            tabControl.SelectedIndex = 2;
        }
    }

    private void LoadData()
    {
        try
        {
            // Load books
            if (File.Exists(booksFile))
            {
                string json = File.ReadAllText(booksFile);
                books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            else
            {
                // Sample books
                books = new List<Book>();
                books.Add(new Book { Id = 1, Title = "C# Programming", Author = "John Doe", Price = 29.99m, Seller = "Alice", IsAvailable = true });
                books.Add(new Book { Id = 2, Title = "Python Basics", Author = "Jane Smith", Price = 24.99m, Seller = "Bob", IsAvailable = true });
                books.Add(new Book { Id = 3, Title = "Java Guide", Author = "Mike Wilson", Price = 34.99m, Seller = "Charlie", IsAvailable = true });
            }
            
            // Load transactions
            if (File.Exists(transactionsFile))
            {
                string json = File.ReadAllText(transactionsFile);
                transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveData()
    {
        try
        {
            // Save books
            var options = new JsonSerializerOptions { WriteIndented = true };
            string booksJson = JsonSerializer.Serialize(books, options);
            File.WriteAllText(booksFile, booksJson);
            
            // Save transactions
            string transactionsJson = JsonSerializer.Serialize(transactions, options);
            File.WriteAllText(transactionsFile, transactionsJson);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving data: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
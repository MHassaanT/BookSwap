using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class BookDetailsWindow : Window
{
    private Book _book;

    // Parameterless constructor for XAML designer
    public BookDetailsWindow()
    {
        InitializeComponent();
        _book = new Book();
    }

    public BookDetailsWindow(Book book) : this()
    {
        _book = book;
        LoadData();
    }

    private void LoadData()
    {
        txtTitle.Text = _book.Title;
        txtAuthor.Text = _book.Author;
        txtCategory.Text = _book.Category;
        txtCondition.Text = _book.Condition;
        txtSeller.Text = _book.Seller;
        txtPrice.Text = _book.DisplayPrice;

        bool isOwnBook = UserService.CurrentUser?.Name == _book.Seller;
        
        if (!_book.IsAvailable)
        {
            txtStatusMsg.Text = "This book is no longer available.";
            btnBuy.IsEnabled = false;
        }
        else if (isOwnBook)
        {
            txtStatusMsg.Text = "This is your listing.";
            btnBuy.IsEnabled = false;
        }
    }

    private void OnBuyClick(object sender, RoutedEventArgs e)
    {
        var checkout = new CheckoutWindow(_book);
        checkout.ShowDialog(this).ContinueWith(t => 
        {
            // If bought, close this window to reflect latest catalog
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => this.Close());
        });
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

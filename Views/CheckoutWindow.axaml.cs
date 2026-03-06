using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class CheckoutWindow : Window
{
    private Book? _bookToBuy;

    public CheckoutWindow()
    {
        InitializeComponent();
    }

    public CheckoutWindow(Book book)
    {
        InitializeComponent();
        _bookToBuy = book;
        LoadData();
    }

    private void LoadData()
    {
        if (_bookToBuy == null) return;
        txtItemName.Text = _bookToBuy.Title;
        txtSellerName.Text = _bookToBuy.Seller;
        txtTotal.Text = _bookToBuy.DisplayPrice;
    }

    private void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        if (_bookToBuy == null) return;
        var currentUser = UserService.CurrentUser;
        if (currentUser == null) return;

        BookService.Purchase(_bookToBuy, currentUser);
        
        // Show success, wait a brief moment maybe? We'll just close it.
        this.Close();
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

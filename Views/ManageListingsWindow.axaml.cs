using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class ManageListingsWindow : Window
{
    public ManageListingsWindow()
    {
        InitializeComponent();
        RefreshData();
    }

    private void RefreshData()
    {
        lstListings.ItemsSource = BookService.LoadBooks();
    }

    private void OnDeleteListingClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int bookId)
        {
            BookService.DeleteBook(bookId);
            RefreshData();
        }
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class AdminDashboard : Window
{
    public AdminDashboard()
    {
        InitializeComponent();
        RefreshStats();
    }

    private void RefreshStats()
    {
        txtTotalUsers.Text = UserService.LoadUsers().Count.ToString();
        
        var books = BookService.LoadBooks();
        txtActiveListings.Text = books.Count(b => b.IsAvailable).ToString();

        var sales = BookService.LoadTransactions();
        txtTotalSales.Text = $"${sales.Sum(t => t.FinalPrice):F2}";
    }

    private void OnManageUsersClick(object sender, RoutedEventArgs e)
    {
        new ManageUsersWindow().ShowDialog(this).ContinueWith(t => 
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(RefreshStats));
    }

    private void OnManageListingsClick(object sender, RoutedEventArgs e)
    {
        new ManageListingsWindow().ShowDialog(this).ContinueWith(t => 
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(RefreshStats));
    }

    private void OnCatalogClick(object sender, RoutedEventArgs e)
    {
        new CatalogWindow().Show();
        this.Close();
    }

    private void OnLogoutClick(object sender, RoutedEventArgs e)
    {
        UserService.Logout();
        new LoginWindow().Show();
        this.Close();
    }
}

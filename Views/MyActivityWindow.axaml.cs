using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class MyActivityWindow : Window
{
    public MyActivityWindow()
    {
        InitializeComponent();
        RefreshData();
    }

    private void RefreshData()
    {
        var currentUser = UserService.CurrentUser;
        if (currentUser == null) return;

        // My Listings
        var books = BookService.LoadBooks()
                    .Where(b => b.Seller == currentUser.Name)
                    .ToList();
        lstMyListings.ItemsSource = books;

        // My Orders
        var tx = BookService.LoadTransactions()
                 .Where(t => t.BuyerID == currentUser.ID.ToString())
                 .OrderByDescending(t => t.Date)
                 .ToList();
        lstMyOrders.ItemsSource = tx;
    }

    private void OnListingDoubleTapped(object sender, TappedEventArgs e)
    {
        if (lstMyListings.SelectedItem is Book selectedBook)
        {
            var win = new AddEditListingWindow(selectedBook);
            win.ShowDialog(this).ContinueWith(t => 
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(RefreshData));
        }
    }
}

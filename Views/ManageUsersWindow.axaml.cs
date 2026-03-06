using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class ManageUsersWindow : Window
{
    public ManageUsersWindow()
    {
        InitializeComponent();
        RefreshData();
    }

    private void RefreshData()
    {
        lstUsers.ItemsSource = UserService.LoadUsers();
    }

    private void OnDeleteUserClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Guid userId)
        {
            // Do not allow admin to delete themselves to prevent lockout
            var currentUser = UserService.CurrentUser;
            if (currentUser != null && currentUser.ID == userId)
            {
                // Optional: show a message
                return;
            }

            UserService.DeleteUser(userId);
            RefreshData();
        }
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

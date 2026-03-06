using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class MyProfileWindow : Window
{
    private User _user;

    public MyProfileWindow()
    {
        InitializeComponent();
        _user = UserService.CurrentUser ?? new User();
        LoadData();
    }

    private void LoadData()
    {
        txtRole.Text = $"{_user.Role} (ID: {_user.ID})";
        txtEmail.Text = _user.Email;
        txtName.Text = _user.Name;
        txtContact.Text = _user.ContactInfo;
        txtPassword.Text = _user.PasswordHash;
        txtStatus.Text = "";
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        _user.Name = txtName.Text?.Trim() ?? "";
        _user.ContactInfo = txtContact.Text?.Trim() ?? "";
        _user.PasswordHash = txtPassword.Text ?? "";

        UserService.UpdateProfile(_user);
        txtStatus.Text = "Profile updated successfully!";
    }

    private void OnDiscardClick(object sender, RoutedEventArgs e)
    {
        LoadData(); // revert UI to existing fields
    }
}

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }

    private void OnRegisterClick(object sender, RoutedEventArgs e)
    {
        txtMessage.Text = "";
        string name = txtName.Text?.Trim() ?? "";
        string email = txtEmail.Text?.Trim() ?? "";
        string password = txtPassword.Text ?? "";
        string contact = txtContact.Text?.Trim() ?? "";
        string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Student";

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMsg("Name, Email, and Password are required.", true);
            return;
        }

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = password,
            ContactInfo = contact,
            Role = role
        };

        var err = UserService.Register(user);
        if (err != null)
        {
            ShowMsg(err, true);
        }
        else
        {
            ShowMsg("Registration Successful! You can now log in.", false);
            // Wait shortly or require user to close manually; we'll let them see the success and close themselves.
        }
    }

    private void ShowMsg(string msg, bool isError)
    {
        txtMessage.Text = msg;
        txtMessage.Foreground = isError ? Brushes.Red : Brushes.Green;
    }
}

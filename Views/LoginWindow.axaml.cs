using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void OnLoginClick(object sender, RoutedEventArgs e)
    {
        txtError.IsVisible = false;
        var email = txtEmail.Text?.Trim() ?? "";
        var password = txtPassword.Text ?? "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter both email and password.");
            return;
        }

        var user = UserService.Authenticate(email, password);
        if (user == null)
        {
            ShowError("Invalid Email or Password.");
            return;
        }

        Window nextWindow;
        if (user.Role == "Admin")
            nextWindow = new AdminDashboard();
        else
            nextWindow = new CatalogWindow();

        nextWindow.Show();
        this.Close();
    }

    private void OnRegisterClick(object sender, RoutedEventArgs e)
    {
        var regWindow = new RegisterWindow();
        regWindow.ShowDialog(this);
    }

    private void ShowError(string msg)
    {
        txtError.Text = msg;
        txtError.IsVisible = true;
    }
}

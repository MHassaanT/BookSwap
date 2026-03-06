using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class CatalogWindow : Window
{
    private List<Book> _allBooks = new();

    public CatalogWindow()
    {
        InitializeComponent();
        RefreshCatalog();
    }

    private void RefreshCatalog()
    {
        _allBooks = BookService.LoadBooks();
        RunFilter();
    }

    private void RunFilter()
    {
        if (txtSearch == null || cmbCategory == null) return;

        string searchText = txtSearch.Text?.ToLower() ?? "";
        string selectedCat = (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All Categories";
        double maxPrice = sldPrice.Value;

        var filtered = _allBooks.Where(b => 
            (string.IsNullOrEmpty(searchText) || b.Title.ToLower().Contains(searchText) || b.Author.ToLower().Contains(searchText)) &&
            (selectedCat == "All Categories" || b.Category == selectedCat) &&
            (double)b.Price <= maxPrice
        ).ToList();

        lstBooks.ItemsSource = filtered;
    }

    private void OnSearchKeyUp(object? sender, KeyEventArgs e) => RunFilter();
    private void OnCategoryChanged(object? sender, SelectionChangedEventArgs e) => RunFilter();
    private void OnSliderChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Value") RunFilter();
    }

    // ── Navigation ──────────────────────────────────────────────────
    private void OnAddListingClick(object sender, RoutedEventArgs e)
    {
        var win = new AddEditListingWindow();
        win.ShowDialog(this).ContinueWith(t => Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(RefreshCatalog));
    }

    private void OnMyActivityClick(object sender, RoutedEventArgs e)
    {
        new MyActivityWindow().ShowDialog(this);
    }

    private void OnMyProfileClick(object sender, RoutedEventArgs e)
    {
        new MyProfileWindow().ShowDialog(this);
    }

    private void OnLogoutClick(object sender, RoutedEventArgs e)
    {
        UserService.Logout();
        new LoginWindow().Show();
        this.Close();
    }

    private void OnBookDoubleTapped(object sender, TappedEventArgs e)
    {
        if (lstBooks.SelectedItem is Book selectedBook)
        {
            var win = new BookDetailsWindow(selectedBook);
            win.ShowDialog(this).ContinueWith(t => Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(RefreshCatalog));
        }
    }
}
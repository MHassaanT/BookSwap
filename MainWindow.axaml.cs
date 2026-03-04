using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Module_2;

public partial class MainWindow : Window
{
    private List<Book> _allBooks = new();

    public MainWindow()
    {
        InitializeComponent();
        LoadSampleData();
        lstBooks.ItemsSource = _allBooks;
    }

    private void LoadSampleData()
    {
        _allBooks = new List<Book>
        {
            new Book { ID = 1, Title = "Calc 101", Author = "Newton", Price = 45.00m, Category = "Engineering", Status = "Available" },
            new Book { ID = 2, Title = "Circuit Analysis", Author = "Tesla", Price = 15.50m, Category = "Engineering", Status = "Available" },
            new Book { ID = 3, Title = "Modern Art History", Author = "Picasso", Price = 30.00m, Category = "Arts", Status = "Available" },
            new Book { ID = 4, Title = "Physics Vol 1", Author = "Einstein", Price = 19.99m, Category = "Science", Status = "Pending" }
        };
    }

    private void RunFilter()
    {
        if (txtSearch == null || cmbCategory == null) return;

        string searchText = txtSearch.Text?.ToLower() ?? "";
        var selectedItem = cmbCategory.SelectedItem as ComboBoxItem;
        string selectedCat = selectedItem?.Content?.ToString() ?? "All Categories";
        double maxPrice = sldPrice.Value;

        var filtered = _allBooks.Where(b => 
            (string.IsNullOrEmpty(searchText) || b.Title.ToLower().Contains(searchText) || b.Author.ToLower().Contains(searchText)) &&
            (selectedCat == "All Categories" || b.Category == selectedCat) &&
            (double)b.Price <= maxPrice
        ).ToList();

        lstBooks.ItemsSource = filtered;
    }

    // Fixed: KeyUp needs KeyEventArgs
    public void OnSearchKeyUp(object? sender, KeyEventArgs e) => RunFilter();

    // Fixed: SelectionChanged needs SelectionChangedEventArgs
    public void OnCategoryChanged(object? sender, SelectionChangedEventArgs e) => RunFilter();

    public void OnSliderChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Value") RunFilter();
    }
}
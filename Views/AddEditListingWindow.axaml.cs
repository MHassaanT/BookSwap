using Avalonia.Controls;
using Avalonia.Interactivity;
using BookSwap.Core;
using BookSwap.Services;

namespace BookSwap.Views;

public partial class AddEditListingWindow : Window
{
    private Book? _editingBook;

    public AddEditListingWindow()
    {
        InitializeComponent();
        txtHeader.Text = "Add New Listing";
    }

    public AddEditListingWindow(Book bookToEdit)
    {
        InitializeComponent();
        _editingBook = bookToEdit;
        txtHeader.Text = "Edit Listing";
        LoadData();
    }

    private void LoadData()
    {
        if (_editingBook == null) return;
        txtTitle.Text = _editingBook.Title;
        txtAuthor.Text = _editingBook.Author;
        txtPrice.Text = _editingBook.Price.ToString("F2");

        SelectComboBoxItem(cmbCategory, _editingBook.Category);
        SelectComboBoxItem(cmbCondition, _editingBook.Condition);
    }

    private void SelectComboBoxItem(ComboBox cmb, string value)
    {
        foreach (ComboBoxItem item in cmb.Items)
        {
            if (item.Content?.ToString() == value)
            {
                cmb.SelectedItem = item;
                break;
            }
        }
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        txtError.Text = "";
        string title = txtTitle.Text?.Trim() ?? "";
        string author = txtAuthor.Text?.Trim() ?? "";
        string priceStr = txtPrice.Text?.Trim() ?? "";
        string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "General";
        string condition = (cmbCondition.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Good";

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(priceStr))
        {
            txtError.Text = "Title, Author, and Price are required.";
            return;
        }

        if (!decimal.TryParse(priceStr, out decimal price) || price < 0)
        {
            txtError.Text = "Please enter a valid positive price.";
            return;
        }

        if (_editingBook == null)
        {
            var newBook = new Book
            {
                Title = title,
                Author = author,
                Category = category,
                Condition = condition,
                Price = price,
                Seller = UserService.CurrentUser?.Name ?? "Unknown",
                Status = "Available",
                IsAvailable = true
            };
            BookService.AddBook(newBook);
        }
        else
        {
            _editingBook.Title = title;
            _editingBook.Author = author;
            _editingBook.Category = category;
            _editingBook.Condition = condition;
            _editingBook.Price = price;
            BookService.UpdateBook(_editingBook);
        }

        this.Close();
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

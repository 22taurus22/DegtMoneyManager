using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Interfaces;
using DegtMoney.Models;
using DegtMoney.Services;
using System.Data.Entity;
using DegtMoney.Views;


namespace DegtMoney.ViewModels
{
    public partial class TransactionsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IEncryptionService _encryption;

        [ObservableProperty] private DateTime _filterStart = DateTime.Now.AddMonths(-1);
        [ObservableProperty] private DateTime _filterEnd = DateTime.Now;
        [ObservableProperty] private Category? _selectedCategory;
        [ObservableProperty] private List<Transaction> _filteredTransactions = new();
        [ObservableProperty] private List<Category> _categories = new();

        public TransactionsViewModel(AppDbContext context, IEncryptionService encryption)
        {
            _context = context;
            _encryption = encryption;
            LoadCategories();
            ApplyFilter();
        }

        private void LoadCategories()
        {
            Categories = _context.Categories.Where(c => c.UserId == SessionService.CurrentUser.Id).ToList();
        }

        [RelayCommand]
        private void ApplyFilter()
        {
            var query = _context.Transactions
                .Where(t => t.UserId == SessionService.CurrentUser.Id && t.Date >= FilterStart && t.Date <= FilterEnd);
            if (SelectedCategory != null)
                query = query.Where(t => t.CategoryId == SelectedCategory.Id);
            var transactions = query.Include(t => t.Category).OrderByDescending(t => t.Date).ToList();
            foreach (var t in transactions)
                t.NoteEncrypted = _encryption.Decrypt(t.NoteEncrypted);
            FilteredTransactions = transactions;
        }

        [RelayCommand]
        private void AddTransaction()
        {
            var dialog = new TransactionDialog(Categories);
            if (dialog.ShowDialog() == true)
            {
                var transaction = new Transaction
                {
                    Amount = dialog.Amount,
                    Date = dialog.Date,
                    NoteEncrypted = _encryption.Encrypt(dialog.Note),
                    CategoryId = dialog.SelectedCategory.Id,
                    UserId = SessionService.CurrentUser.Id
                };
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                ApplyFilter();
            }
        }

        [RelayCommand]
        private void EditTransaction(Transaction transaction)
        {
            var originalNote = _encryption.Decrypt(transaction.NoteEncrypted);
            var dialog = new TransactionDialog(Categories, transaction.Amount, transaction.Date, originalNote, transaction.Category);
            if (dialog.ShowDialog() == true)
            {
                transaction.Amount = dialog.Amount;
                transaction.Date = dialog.Date;
                transaction.NoteEncrypted = _encryption.Encrypt(dialog.Note);
                transaction.CategoryId = dialog.SelectedCategory.Id;
                _context.SaveChanges();
                ApplyFilter();
            }
        }

        [RelayCommand]
        private void DeleteTransaction(Transaction transaction)
        {
            if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
                ApplyFilter();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Models;
using DegtMoney.Services;
using DegtMoney.Views;

namespace DegtMoney.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        [ObservableProperty] private List<Category> _incomeCategories = new();
        [ObservableProperty] private List<Category> _expenseCategories = new();

        public CategoriesViewModel(AppDbContext context)
        {
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            var all = _context.Categories.Where(c => c.UserId == SessionService.CurrentUser.Id).ToList();
            IncomeCategories = all.Where(c => c.IsIncome).ToList();
            ExpenseCategories = all.Where(c => !c.IsIncome).ToList();
        }

        [RelayCommand]
        private void AddIncomeCategory()
        {
            var newCat = new Category { Name = "Новый доход", IsIncome = true, UserId = SessionService.CurrentUser.Id };
            _context.Categories.Add(newCat);
            _context.SaveChanges();
            LoadCategories();
        }

        [RelayCommand]
        private void AddExpenseCategory()
        {
            var newCat = new Category { Name = "Новый расход", IsIncome = false, UserId = SessionService.CurrentUser.Id };
            _context.Categories.Add(newCat);
            _context.SaveChanges();
            LoadCategories();
        }

        [RelayCommand]
        private void DeleteCategory(Category cat)
        {
            if (_context.Transactions.Any(t => t.CategoryId == cat.Id))
                MessageBox.Show("Нельзя удалить категорию с транзакциями");
            else
            {
                _context.Categories.Remove(cat);
                _context.SaveChanges();
                LoadCategories();
            }
        }

        [RelayCommand]
        private void EditCategory(Category cat)
        {
            var dialog = new InputDialog("Новое название", cat.Name);
            if (dialog.ShowDialog() == true)
            {
                cat.Name = dialog.Answer;
                _context.SaveChanges();
                LoadCategories();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BCrypt.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Models;
using DegtMoney.Services;
using BCryptNet = BCrypt.Net.BCrypt;
using DegtMoney.Views;


namespace DegtMoney.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        [ObservableProperty] private string _username = "";
        [ObservableProperty] private string _email = "";
        [ObservableProperty] private string _password = "";
        [ObservableProperty] private string _confirmPassword = "";

        public RegisterViewModel(AppDbContext context)
        {
            _context = context;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            if (_context.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("Пользователь уже существует");
                return;
            }

            var user = new User
            {
                Username = Username,
                Email = Email,
                PasswordHash = BCryptNet.HashPassword(Password),
                CurrencyCode = "RUB",
                IsDarkTheme = false,
                LanguageCode = "ru"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var defaultCategories = new[]
            {
                new Category { Name = "Зарплата", IsIncome = true, UserId = user.Id },
                new Category { Name = "Фриланс", IsIncome = true, UserId = user.Id },
                new Category { Name = "Еда", IsIncome = false, UserId = user.Id },
                new Category { Name = "Транспорт", IsIncome = false, UserId = user.Id },
                new Category { Name = "ЖКХ", IsIncome = false, UserId = user.Id },
                new Category { Name = "Развлечения", IsIncome = false, UserId = user.Id }
            };
            _context.Categories.AddRange(defaultCategories);
            await _context.SaveChangesAsync();

            MessageBox.Show("Регистрация успешна");
            foreach (Window window in Application.Current.Windows)
                if (window is RegisterWindow)
                {
                    window.Close();
                    break;
                }
        }
    }
}

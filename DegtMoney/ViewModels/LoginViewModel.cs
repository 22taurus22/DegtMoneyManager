using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BCrypt.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Services;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;
using DegtMoney.Views;
using DegtMoney;

namespace DegtMoney.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly ReminderService _reminderService;

        [ObservableProperty] private string _username = "";
        [ObservableProperty] private string _password = "";

        public LoginViewModel(AppDbContext context, ReminderService reminderService)
        {
            _context = context;
            _reminderService = reminderService;
        }

        [RelayCommand]
        private async Task Login()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null || !BCryptNet.Verify(Password, user.PasswordHash))
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            SessionService.CurrentUser = user;
            _reminderService.StartForUser(user.Id);

            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0]?.Close();
        }

        [RelayCommand]
        private void OpenRegister()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}

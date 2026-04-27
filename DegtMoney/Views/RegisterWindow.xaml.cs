using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DegtMoney.Services;
using DegtMoney.ViewModels;

namespace DegtMoney.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var vm = new RegisterViewModel(new AppDbContext());
            this.DataContext = vm;
            PasswordBox.PasswordChanged += (s, e) => vm.Password = PasswordBox.Password;
            ConfirmPasswordBox.PasswordChanged += (s, e) => vm.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
}

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
using DegtMoney.ViewModels;

namespace DegtMoney.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (this.DataContext is LoginViewModel vm)
                {
                    vm.Password = "";
                    PasswordBox.PasswordChanged += (sender, args) => vm.Password = PasswordBox.Password;
                }
            };
        }
    }
}

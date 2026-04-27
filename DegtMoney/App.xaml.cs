using System.Configuration;
using System.Data;
using System.Windows;
using DegtMoney.Services;
using DegtMoney.ViewModels;
using DegtMoney.Views;

namespace DegtMoney
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var context = new AppDbContext();
            var reminder = new ReminderService(context);
            var loginVM = new LoginViewModel(context, reminder);
            var loginWindow = new LoginWindow { DataContext = loginVM };
            loginWindow.Show();
        }
    }

}

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
using System.Windows.Navigation;
using DegtMoney.Views;

namespace DegtMoney.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var context = new AppDbContext();
            var reportService = new ReportService(context, new AesEncryptionService());
            var notification = new ReminderService(context);
            this.DataContext = new MainViewModel(context, reportService, notification);
        }
    }
}

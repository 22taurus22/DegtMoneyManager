using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Interfaces;
using DegtMoney.Services;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.Data.Entity;
using DegtMoney.Views;


namespace DegtMoney.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IReportService _reportService;
        private readonly INotificationService _notificationService;

        [ObservableProperty] private decimal _totalIncome;
        [ObservableProperty] private decimal _totalExpense;
        [ObservableProperty] private SeriesCollection _pieChartSeries;
        [ObservableProperty] private DateTime _startDate = DateTime.Now.AddMonths(-1);
        [ObservableProperty] private DateTime _endDate = DateTime.Now;
        [ObservableProperty] private string _currentCurrency = "RUB";

        public MainViewModel(AppDbContext context, IReportService reportService, INotificationService notificationService)
        {
            _context = context;
            _reportService = reportService;
            _notificationService = notificationService;
            CurrentCurrency = SessionService.CurrentUser?.CurrencyCode ?? "RUB";
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var transactions = _context.Transactions
                .Where(t => t.UserId == SessionService.CurrentUser.Id && t.Date >= StartDate && t.Date <= EndDate)
                .Include(t => t.Category)
                .ToList();

            TotalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            TotalExpense = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);

            var expenseByCategory = transactions
                .Where(t => !t.Category.IsIncome)
                .GroupBy(t => t.Category.Name)
                .Select(g => new PieSeries { Title = g.Key, Values = new ChartValues<decimal> { g.Sum(t => t.Amount) } })
                .ToList();
            PieChartSeries = new SeriesCollection(expenseByCategory);
        }

        [RelayCommand] private void Refresh() => LoadStatistics();

        [RelayCommand]
        private void OpenTransactions()
        {
            var vm = new TransactionsViewModel(_context, new AesEncryptionService());
            var win = new TransactionsWindow { DataContext = vm };
            win.ShowDialog();
        }

        [RelayCommand]
        private void OpenCategories()
        {
            var vm = new CategoriesViewModel(_context);
            var win = new CategoriesWindow { DataContext = vm };
            win.ShowDialog();
        }

        [RelayCommand]
        private void OpenSavingGoals()
        {
            var vm = new SavingGoalsViewModel(_context, _notificationService);
            var win = new SavingGoalsWindow { DataContext = vm };
            win.ShowDialog();
        }

        [RelayCommand]
        private void OpenReports()
        {
            var vm = new ReportViewModel(_reportService);
            var win = new ReportWindow { DataContext = vm };
            win.ShowDialog();
        }

        [RelayCommand]
        private void OpenSettings()
        {
            var vm = new SettingsViewModel(_context);
            var win = new SettingsWindow { DataContext = vm };
            win.ShowDialog();
            CurrentCurrency = SessionService.CurrentUser?.CurrencyCode ?? "RUB";
        }

        [RelayCommand]
        private async Task ExportCsv()
        {
            var data = await _reportService.GenerateReportCsvAsync(SessionService.CurrentUser.Id, StartDate, EndDate);
            var saveDialog = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (saveDialog.ShowDialog() == true)
            {
                await System.IO.File.WriteAllBytesAsync(saveDialog.FileName, data);
                _notificationService.ShowToast("Экспорт выполнен");
            }
        }
    }
}

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
using Microsoft.Win32;

namespace DegtMoney.ViewModels
{
    public partial class ReportViewModel : ObservableObject
    {
        private readonly IReportService _reportService;
        [ObservableProperty] private DateTime _startDate = DateTime.Now.AddMonths(-1);
        [ObservableProperty] private DateTime _endDate = DateTime.Now;
        [ObservableProperty] private bool _isLoading;

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        [RelayCommand]
        private async Task ExportCsv()
        {
            await ExportReport("CSV files (*.csv)|*.csv", _reportService.GenerateReportCsvAsync);
        }

        [RelayCommand]
        private async Task ExportPdf()
        {
            await ExportReport("PDF files (*.pdf)|*.pdf", _reportService.GenerateReportPdfAsync);
        }

        private async Task ExportReport(string filter, Func<int, DateTime, DateTime, Task<byte[]>> generateFunc)
        {
            if (SessionService.CurrentUser == null) return;
            var saveDialog = new SaveFileDialog { Filter = filter };
            if (saveDialog.ShowDialog() == true)
            {
                IsLoading = true;
                try
                {
                    var data = await generateFunc(SessionService.CurrentUser.Id, StartDate, EndDate);
                    await System.IO.File.WriteAllBytesAsync(saveDialog.FileName, data);
                    MessageBox.Show("Отчёт сохранён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
    }
}

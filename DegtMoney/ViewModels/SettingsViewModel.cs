using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Services;
namespace DegtMoney.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        [ObservableProperty] private string[] _currencies = { "RUB", "USD", "EUR" };
        [ObservableProperty] private string _selectedCurrency;
        [ObservableProperty] private bool _isDarkTheme;
        [ObservableProperty] private string[] _languages = { "ru", "en" };
        [ObservableProperty] private string _selectedLanguage;

        public SettingsViewModel(AppDbContext context)
        {
            _context = context;
            var user = SessionService.CurrentUser;
            SelectedCurrency = user.CurrencyCode;
            IsDarkTheme = user.IsDarkTheme;
            SelectedLanguage = user.LanguageCode;
        }

        [RelayCommand]
        private void Save()
        {
            var user = SessionService.CurrentUser;
            user.CurrencyCode = SelectedCurrency;
            user.IsDarkTheme = IsDarkTheme;
            user.LanguageCode = SelectedLanguage;
            _context.SaveChanges();

            var dict = new ResourceDictionary();
            dict.Source = IsDarkTheme ? new Uri("Themes/Dark.xaml", UriKind.Relative) : new Uri("Themes/Light.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SelectedLanguage);
            MessageBox.Show("Настройки сохранены");
        }
    }
}

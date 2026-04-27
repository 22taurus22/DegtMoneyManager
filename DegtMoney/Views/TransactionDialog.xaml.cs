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
using DegtMoney.Models;

namespace DegtMoney.Views
{
    /// <summary>
    /// Логика взаимодействия для TransactionDialog.xaml
    /// </summary>
    public partial class TransactionDialog : Window
    {
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; } = "";
        public Category SelectedCategory { get; private set; }

        private readonly List<Category> _categories;

        public TransactionDialog(List<Category> categories, decimal amount = 0, DateTime? date = null, string note = "", Category selectedCategory = null)
        {
            InitializeComponent();
            _categories = categories;
            CategoryCombo.ItemsSource = categories;
            if (selectedCategory != null)
                CategoryCombo.SelectedItem = selectedCategory;
            else if (categories.Any())
                CategoryCombo.SelectedItem = categories.First();
            AmountBox.Text = amount.ToString();
            DatePicker.SelectedDate = date ?? DateTime.Today;
            NoteBox.Text = note;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму");
                return;
            }
            if (CategoryCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
            Amount = amount;
            Date = DatePicker.SelectedDate ?? DateTime.Today;
            Note = NoteBox.Text;
            SelectedCategory = (Category)CategoryCombo.SelectedItem;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

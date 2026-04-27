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

namespace DegtMoney.Views
{
    /// <summary>
    /// Логика взаимодействия для GoalDialog.xaml
    /// </summary>
    public partial class GoalDialog : Window
    {
        public string Name => NameBox.Text;
        public decimal TargetAmount { get; private set; }
        public DateTime Deadline => DeadlinePicker.SelectedDate ?? DateTime.Today.AddMonths(1);

        public GoalDialog()
        {
            InitializeComponent();
            DeadlinePicker.SelectedDate = DateTime.Today.AddMonths(1);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (!decimal.TryParse(AmountBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму");
                return;
            }
            TargetAmount = amount;
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

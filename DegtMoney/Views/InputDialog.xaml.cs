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
    /// Логика взаимодействия для InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string Answer { get; private set; } = "";

        public InputDialog(string prompt, string defaultValue = "")
        {
            InitializeComponent();
            PromptText.Text = prompt;
            AnswerBox.Text = defaultValue;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Answer = AnswerBox.Text;
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

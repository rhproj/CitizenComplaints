using Complaints_WPF.Authorization;
using Complaints_WPF.ViewModels;
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

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string ProsNamo { get; set; }

        public LoginWindow()
        {
            InitializeComponent();

            btnClose.Click += (s, e) => Close();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (comBoxProsecutor.SelectedItem != null)
            {
                //ProsecutorsAccess.ProsecutorsLogin = comBoxProsecutor.SelectedItem.ToString();
                MainWindow mw = new MainWindow();
                //mw. //lblPros.Content = comBoxProsecutor.SelectedItem;
                mw.Show();
                ComplaintsViewModel.ProsecName = comBoxProsecutor.SelectedItem.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не выбран пользователь");
            }

        }
    }
}

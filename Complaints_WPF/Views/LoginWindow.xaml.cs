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
        //public string ProsNamo { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            
            tbYear.Text = DateTime.Now.Year.ToString();  //some day you'll need to do it mvvm way

            btnClose.Click += (s, e) => Close();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (comBoxProsecutor.SelectedItem != null)
            {
                //ComplaintsViewModel.CurrentYear = tbYear.Text;
                
                MainWindow mw = new MainWindow();
                mw.lblProsecutor.Content = comBoxProsecutor.SelectedItem;
                //mw.lblChief.Content = comBoxСhief.SelectedItem;

                ComplaintsViewModel.YearToFilter = tbYear.Text; //!!## walkaround the date problem, i'm passing it here!
                ComplaintsViewModel.ProsecutorLogin = comBoxProsecutor.SelectedItem.ToString(); //passing selected pros to VM, so it could be written to dataGridTable in Insert method
                
                //if (comBoxСhief.SelectedItem != null)
                //{
                //    ComplaintsViewModel.СhiefProsecutor = comBoxСhief.SelectedItem.ToString(); //29.12
                //}
                //else
                //{
                //    mw.lblByChief.Content = string.Empty;
                //}

                mw.Show();
                //this.Hide();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не выбран пользователь");
            }

        }
    }
}

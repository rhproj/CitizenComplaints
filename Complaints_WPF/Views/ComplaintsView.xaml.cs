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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for ComplaintsView.xaml
    /// </summary>
    public partial class ComplaintsView : UserControl
    {
        public ComplaintsView()
        {
            InitializeComponent();
        }

        private void Btn_Constructor_Click(object sender, RoutedEventArgs e)
        {
            ComboConstructor comboConstr = new ComboConstructor();
            comboConstr.ShowDialog();
        }

        private void Btn_SummOZh_Click(object sender, RoutedEventArgs e)
        {
            SummOZhView summOZhView = new SummOZhView();
            summOZhView.ShowDialog();
        }

        //private void Btn_Save_Click(object sender, RoutedEventArgs e)  //trying to do it MVVM way
        //{

        //}
    }
}

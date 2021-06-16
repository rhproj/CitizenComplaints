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
            ComboEditView comboConstr = new ComboEditView();
            comboConstr.ShowDialog();
        }
    }
}

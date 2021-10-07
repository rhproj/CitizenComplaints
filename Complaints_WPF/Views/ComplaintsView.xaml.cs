using System.Windows;
using System.Windows.Controls;

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

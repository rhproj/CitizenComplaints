using System.Windows;

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for SummOZhView.xaml
    /// </summary>
    public partial class SummOZhView : Window
    {
        public SummOZhView()
        {
            InitializeComponent();
            btnClose.Click += (s,e) => this.Close();
        }
    }
}

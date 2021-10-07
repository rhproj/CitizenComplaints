using System.Windows;

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for ComboEditView.xaml
    /// </summary>
    public partial class ComboEditView : Window
    {
        public ComboEditView()
        {
            InitializeComponent();
            btnClose.Click += (s, e) => Close();
        }
    }
}

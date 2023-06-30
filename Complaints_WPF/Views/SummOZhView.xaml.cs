using Complaints_WPF.Services.Interfaces;
using Complaints_WPF.ViewModels;
using System.Windows;

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for SummOZhView.xaml
    /// </summary>
    public partial class SummOZhView : Window
    {
        public SummOZhView(IOZhReadService oZhReadService)
        {
            InitializeComponent();
            DataContext = new SummOZhViewModel(oZhReadService);

            btnClose.Click += (s,e) => this.Close();
        }
    }
}

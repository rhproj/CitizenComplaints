using Complaints_WPF.ViewModels;
using System;
using System.Windows;

namespace Complaints_WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(ComplaintsViewModel viewModel)
        {
            InitializeComponent();            
            DataContext = viewModel;

            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(this.Close);

            btnClose.Click += (s, e) => Close();
        }
    }
}

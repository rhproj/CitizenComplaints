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
        ComplaintsViewModel complaintsVM;

        public LoginWindow(ComplaintsViewModel viewModel)
        {
            InitializeComponent();            
            //tbYear.Text = DateTime.Now.Year.ToString();
            complaintsVM = viewModel; 
            DataContext = complaintsVM;

            if (complaintsVM.CloseAction == null)
                complaintsVM.CloseAction = new Action(this.Close);

            btnClose.Click += (s, e) => Close();
        }
    }
}

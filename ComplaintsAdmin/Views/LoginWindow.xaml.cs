using Complaints_WPF.ViewModels;
using ComplaintsAdmin.ViewModels;
using System;
using System.Windows;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            btnClose.Click += (s, e) => Close();
        }
    }
}


    //public LoginWindow(ComplaintsViewModel viewModel)
    //{
    //    InitializeComponent();
    //    DataContext = viewModel;
    
    //    if (viewModel.CloseAction == null)
    //        viewModel.CloseAction = new Action(this.Close);
    
    //    btnClose.Click += (s, e) => Close();
    //}
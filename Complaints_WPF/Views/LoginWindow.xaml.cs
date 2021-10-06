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

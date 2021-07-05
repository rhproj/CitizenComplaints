using ComplaintsAdmin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComplaintsAdmin.Commands
{
    class EditUsersOpenCommand : ICommand
    {
        private EditUsersView _editUsersView;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = new EditUsersView(); //owner?
            window.Show(); //not ShowDialog or mother W won't be closed!

            (parameter as System.Windows.Window).Close();
            #region the other way to close the w is to search thru and find it by xaml name
            //foreach (Window win in Application.Current.Windows)
            //{
            //    if (win is LoginWindow)
            //    {
            //        win.Close();
            //        break;
            //    }
            //} 
            #endregion
        }

        public event EventHandler CanExecuteChanged;
    }
}

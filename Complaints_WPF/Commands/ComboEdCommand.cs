using Complaints_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Complaints_WPF.Commands
{/// <summary>
/// Command opens ComboEditView window
/// </summary>
    class ComboEdCommand : ICommand
    {
        private ComboEditView _comboEditView;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = new ComboEditView
            {
                Owner = Application.Current.MainWindow
            };

            _comboEditView = window;

            window.Closed += OnWindowClosed;

            window.ShowDialog();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _comboEditView = null;
        }

        public event EventHandler CanExecuteChanged;
    }
}

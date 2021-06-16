using Complaints_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Complaints_WPF.Commands
{
    class SummOZhCommand : ICommand
    {
        private SummOZhView _summOZhView;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = new SummOZhView
            {
                Owner = Application.Current.MainWindow
            };

            _summOZhView = window;

            window.Closed += OnWindowClosed;

            window.ShowDialog();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _summOZhView = null;
        }

        public event EventHandler CanExecuteChanged;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Complaints_WPF.Commands
{
    public class RelayCommand : ICommand
    {
        private Action methodToExecute;
        private Func<bool> canExecuteEvaluator;

        public RelayCommand(Action method, Func<bool> evaluator)
        {
            methodToExecute = method;
            canExecuteEvaluator = evaluator;
        }

        public event EventHandler CanExecuteChanged //add/remove needed to enable/disable buttons (CanExecute uses it) //more: stackoverflow.com/questions/30002300/how-to-use-the-canexecute-method-from-icommand-on-wpf
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            //return true;
            if (canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = canExecuteEvaluator.Invoke();
                return result; //better just: return canExecuteEvaluator(); //<-- try it later
            }
        }

        public void Execute(object parameter)
        {
            methodToExecute(); //method will be run from here //methodToExecute.Invoke();
        }
    }
}

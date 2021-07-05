using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComplaintsAdmin.Commands
{
    public class RelayCommand //: ICommand  //Delete
    {
        //private Action methodToExecute;
        //private Func<bool> canExecuteEvaluator;

        //public RelayCommand(Action method, Func<bool> evaluator)
        //{
        //    methodToExecute = method;
        //    canExecuteEvaluator = evaluator;
        //}

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public bool CanExecute(object parameter)
        //{
        //    if (canExecuteEvaluator == null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        bool result = canExecuteEvaluator.Invoke();
        //        return result; //shorter: just return canExecuteEvaluator();
        //    }
        //}

        //public void Execute(object parameter)
        //{
        //    methodToExecute();
        //}

    }
}

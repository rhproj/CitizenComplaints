﻿using System;
using System.Windows.Input;

namespace Complaints_WPF.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action methodToExecute;
        private readonly Func<bool> canExecuteEvaluator;

        public RelayCommand(Action method, Func<bool> evaluator)
        {
            methodToExecute = method;
            canExecuteEvaluator = evaluator;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = canExecuteEvaluator.Invoke();
                return result;
            }
        }

        public void Execute(object parameter)
        {
            methodToExecute();
        }
    }
}

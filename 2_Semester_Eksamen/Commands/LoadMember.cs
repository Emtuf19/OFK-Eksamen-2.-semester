using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _2_Semester_Eksamen.Commands
{
    public class LoadMember : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object paramiter)
        {
            return true;
        }

        public void Execute(object paramiter)
        {

        }
    }
}

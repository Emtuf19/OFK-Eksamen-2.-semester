using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _2_Semester_Eksamen.ViewModel
{
    public abstract class RegistrerViewModelBase<TEntity> : ViewModelBase 
    where TEntity : class, new()
    {
        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}

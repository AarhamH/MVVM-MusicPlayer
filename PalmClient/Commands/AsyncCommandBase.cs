using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PalmClient.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        protected bool PreventClicksWhileExecuting { get; set; } = true;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                if (PreventClicksWhileExecuting)
                {
                    _isExecuting = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object? parameter)
        {
            IsExecuting = true;
            await ExecuteAsync(parameter);
            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object? parameter);
    }
}

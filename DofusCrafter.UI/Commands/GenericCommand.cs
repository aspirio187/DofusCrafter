using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.Commands
{
    public class GenericCommand : ICommand
    {
        public event Action Command;

        public event EventHandler? CanExecuteChanged;

        public GenericCommand(Action command)
        {
            Command = command;
        }

        public bool CanExecute(object? parameter)
        {
            return Command is not null;
        }

        public void Execute(object? parameter)
        {
            Command?.Invoke();
        }
    }
}

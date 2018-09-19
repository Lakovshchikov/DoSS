using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Doss.Model
{
    class Command : ICommand
    {
        private Action _command;
        private Func<bool> _canExecute;
        public Command(Action command, Func<bool> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        /// <summary>Определяет метод, который определяет, может ли данная команда выполняться в ее текущем состоянии.</summary>
        /// <returns>Значение true, если команда может быть выполнена; в противном случае — значение false..</returns>
        /// <param name="parameter">Данные, используемые данной командой.Если для данной команды не требуется передача данных, можно присвоить этому объекту значение null.</param>
        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute() : false;
        }

        /// <summary>Определяет метод, вызываемый при вызове данной команды.</summary>
        /// <param name="parameter">Данные, используемые данной командой.Если для данной команды не требуется передача данных, можно присвоить этому объекту значение null.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(null))
                _command();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

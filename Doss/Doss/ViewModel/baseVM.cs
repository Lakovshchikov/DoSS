using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Doss.Model;

namespace Doss.ViewModel
{
    class baseVM : DependencyObject
    {
        public Command OkayCommand { get; set; }
        public Command CancelCommand { get; set; }

        public baseVM(Action ok, Action close)
        {
            OkayCommand = new Command(ok, () => true);
            CancelCommand = new Command(close, () => true);
        }
    }
}

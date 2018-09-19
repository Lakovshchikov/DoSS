using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doss.Model;

namespace Doss.ViewModel
{
    class MainVM : baseVM
    {


        #region prop

        #endregion
        public MainVM(Action ok, Action close) : base(ok, close)
        {
            WorkWithJson wr = new WorkWithJson();
        }
    }
}

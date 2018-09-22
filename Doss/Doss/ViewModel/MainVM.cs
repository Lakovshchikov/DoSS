using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doss.Model;
using Esri.ArcGISRuntime.Mapping;

namespace Doss.ViewModel
{
    class MainVM : baseVM
    {

        private Map _map;
        #region prop
        public Map _Map { get { return _map; } set { _map = value; } }
        #endregion
        public MainVM(Action ok, Action close) : base(ok, close)
        {
            _Map = new MapVM().Map;
            WorkWithJson wr = new WorkWithJson();
        }
    }
}

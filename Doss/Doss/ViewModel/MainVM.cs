using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doss.Model;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Doss;

namespace Doss.ViewModel
{
    class MainVM : baseVM, INotifyPropertyChanged
    {

        private Map _map;
        private MapVM _MapVM;
        private MapView _MyMapView;
        #region prop
        public Map Map { get { return _map; } set { _map = value; OnPropertyChanged(); } }
        public MapVM MapViewModel { get { return _MapVM; } set { _MapVM = value; } }

        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; } }
        #endregion
        public MainVM(Action ok, Action close, MapView MyMapViewFormMV) : base(ok, close)
        {
            MyMapView = MyMapViewFormMV;
            MapViewModel = new MapVM(MyMapView);
            Map = MapViewModel.Map;
            MyMapView.GeoViewTapped += MapViewModel.SetLocation;
            
        }

        #region command
        #endregion


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.UI;


namespace Doss.ViewModel
{
    class MapVM : INotifyPropertyChanged
    {
        public MapVM()
        {
            var serviceUri = new Uri("https://pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer");
            ArcGISMapImageLayer imageLayer = new ArcGISMapImageLayer(serviceUri);
            Map.Basemap.BaseLayers.Add(imageLayer);
            Map.InitialViewpoint = new Viewpoint(54.5293000, 36.2754200, 60000);
        }

        //private string _St;
        //public string St { get { return _St; } set { _St = value; } }



        private Map _map = new Map(Basemap.CreateStreets());


        /// <summary>
        /// Gets or sets the map
        /// </summary>
        public Map Map
        {
            get { return _map; }
            set { _map = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

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
using Esri.ArcGISRuntime.UI.Controls;
using Doss;
using Esri.ArcGISRuntime.UI;
using System.Reflection;
using Doss.Model;

namespace Doss.ViewModel
{
    class MapVM : INotifyPropertyChanged
    {

        private Map _map = new Map(Basemap.CreateStreets());
        private MapView _MyMapView;
        private MapPoint _SelectedLocation;
        private GraphicsOverlay _OverLay;
        private PlaceCoord _SelectedPlaceCoord;
        private GetPlaceCoord _GetPlaceCoord;
        private string _Coord;

        #region prop
        public Map Map
        {
            get { return _map; }
            set { _map = value; OnPropertyChanged(); }
        }

        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; OnPropertyChanged(); } }
        public MapPoint SelectedLocation { get { return _SelectedLocation; } set { _SelectedLocation = value;OnPropertyChanged(); } }
        public GraphicsOverlay OverLay { get { return _OverLay; } set { _OverLay = value;OnPropertyChanged(); } }
        public PlaceCoord SelectedPlaceCoord { get { return _SelectedPlaceCoord; } set { _SelectedPlaceCoord = value; OnPropertyChanged(); } }
        public GetPlaceCoord GetPlaceCoordProp { get { return _GetPlaceCoord; } set { _GetPlaceCoord = value; } } 
        public string Coord { get { return _Coord; } set { _Coord = value; } }
        #endregion

        public MapVM(MapView MyMapViewFormWin)
        {
            MyMapView = MyMapViewFormWin;
            var serviceUri = new Uri("https://pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer");
            ArcGISMapImageLayer imageLayer = new ArcGISMapImageLayer(serviceUri);
            Map.Basemap.BaseLayers.Add(imageLayer);
            Map.InitialViewpoint = new Viewpoint(54.5293000, 36.2754200, 60000);
            OverLay = new GraphicsOverlay();
            SelectedPlaceCoord = new PlaceCoord();
            GetPlaceCoordProp = new GetPlaceCoord();
            MyMapView.GraphicsOverlays.Add(OverLay);
        }

        public async void SetLocation(object sender, GeoViewInputEventArgs e)
        {
            SelectedLocation = e.Location;
            ToWGS84();
            await CreatePictureMarker(OverLay);
            SelectedPlaceCoord = GetPlaceCoordProp.GetPlaceCoordMethod(Coord.Substring(0, 9), Coord.Substring(12, 9)).Result;
            
        }

        private void ToWGS84()
        {
            Coord = CoordinateFormatter.ToLatitudeLongitude(SelectedLocation, LatitudeLongitudeFormat.DecimalDegrees, 6);
        }

        public async Task CreatePictureMarker(GraphicsOverlay overlay)
        {
            overlay.Graphics.Clear();
            var currentAssembly = Assembly.GetExecutingAssembly();
            var resourceStream = currentAssembly.GetManifestResourceStream(
                "Doss.Resources.marker.png");
            PictureMarkerSymbol pinSymbol = await PictureMarkerSymbol.CreateAsync(resourceStream);
            pinSymbol.Width = 40;
            pinSymbol.Height = 40;
            MapPoint pinPoint = new MapPoint(SelectedLocation.X, SelectedLocation.Y, SpatialReferences.WebMercator);
            Graphic pinGraphic = new Graphic(pinPoint, pinSymbol);
            overlay.Graphics.Add(pinGraphic);
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

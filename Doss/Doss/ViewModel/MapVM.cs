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
using System.Windows;
using Esri.ArcGISRuntime.UI;
using System.Reflection;
using Doss.Model;
using Doss.Model2;
using System.Threading;
using System.IO;

namespace Doss.ViewModel
{
    class MapVM : INotifyPropertyChanged
    {
        

        private Map _map;
        private MapView _MyMapView;
        private MapPoint _SelectedLocation;
        private GraphicsOverlay _OverLay;
        private PlaceCoord _SelectedPlaceCoord;
        private GetPlace _GetPlace;
        private string _Coord;
        private Place _place;
        private MainVM _MainViewModel;
        private bool _IsEnabled_Cad_Map;
        private bool _IsEnabled_Street_Map;
        private bool _IsEnabled_Space_Map;
        private bool _IsEnabled_OpenStreet_Map;
        private Uri serviceUri;

        #region prop
        public Map Map {get { return _map; }set { _map = value; OnPropertyChanged(); }}
        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; OnPropertyChanged(); } }
        public MapPoint SelectedLocation { get { return _SelectedLocation; } set { _SelectedLocation = value;OnPropertyChanged(); } }
        public GraphicsOverlay OverLay { get { return _OverLay; } set { _OverLay = value;OnPropertyChanged(); } }
        public PlaceCoord SelectedPlaceCoord { get { return _SelectedPlaceCoord; } set { _SelectedPlaceCoord = value; OnPropertyChanged(); } }
        public GetPlace GetPlaceProp { get { return _GetPlace; } set { _GetPlace = value; } } 
        public string Coord { get { return _Coord; } set { _Coord = value; } }
        public Place _Place { get { return _place; } set { _place = value; OnPropertyChanged(); } }
        public MainVM MainViewModel { get { return _MainViewModel; } set { _MainViewModel = value; } }
        public bool IsEnabled_Cad_Map { get { return _IsEnabled_Cad_Map; } set { _IsEnabled_Cad_Map = value; } }
        public bool IsEnabled_Street_Map { get { return _IsEnabled_Street_Map; } set { _IsEnabled_Street_Map = value;  } }
        public bool IsEnabled_Space_Map { get { return _IsEnabled_Space_Map; } set { _IsEnabled_Space_Map = value; } }
        public bool IsEnabled_OpenStreet_Map { get { return _IsEnabled_OpenStreet_Map; } set { _IsEnabled_OpenStreet_Map = value; } }
        #endregion


        #region propdp

        #endregion

        public MapVM(MapView MyMapViewFormWin, MainVM mainViewModel)
        {
            MyMapView = MyMapViewFormWin;
            MainViewModel = mainViewModel;
            MyMapView.Map = new Map(Basemap.CreateImageryWithLabels());
            MainViewModel.SpaceMap = true;
            Load_Cad();
            MainViewModel.CadMap = true;
            MyMapView.Map.InitialViewpoint = new Viewpoint(54.5293000, 36.2754200, 60000);
            OverLay = new GraphicsOverlay();
            SelectedPlaceCoord = new PlaceCoord();
            GetPlaceProp = new GetPlace();
            MyMapView.GraphicsOverlays.Add(OverLay);
            
        }


        public async void SetLocation(object sender, GeoViewInputEventArgs e)
        {

            SelectedLocation = e.Location;          
            ToWGS84();          
            SelectedPlaceCoord = GetPlaceProp.GetPlaceCoordMethod(Coord.Substring(0, 9), Coord.Substring(12, 9));
            _Place = GetPlaceProp.Get_Place(SelectedPlaceCoord.Features[0].Attrs.Id);
            MainViewModel.UpdateInfo();
            MainViewModel.UpdateImg(MainViewModel.BBox_Left_Top.X, MainViewModel.BBox_Right_Bottom.X, MainViewModel.BBox_Right_Bottom.Y, MainViewModel.BBox_Left_Top.Y, MainViewModel.MapViewModel._Place, ((int)MainViewModel.MyWindow.GridMap.ActualWidth).ToString(), ((int)MainViewModel.MyWindow.GridMap.ActualHeight).ToString());
            await CreatePictureMarker(OverLay);
        }

        #region SelectMap
        public void Load_ImageryWithLabels_Layer()
        {
            MainViewModel.SetBBox();  
            MyMapView.Map = new Map(Basemap.CreateImageryWithLabels());
            MyMapView.SetViewpointCenterAsync(MainViewModel.CenterPoint);
            MainViewModel.SetBBox();
        }
        public void Load_StreetMap()
        {
            MainViewModel.SetBBox();
            MyMapView.Map = new Map(Basemap.CreateStreets());
            MyMapView.SetViewpointCenterAsync(MainViewModel.CenterPoint);
            MainViewModel.SetBBox();
        }
        public void Load_Cad()
        {
            serviceUri = new Uri("https://pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/CadastreWMS/MapServer");
            ArcGISMapImageLayer Cad_Layer = new ArcGISMapImageLayer(serviceUri);
            MyMapView.Map.Basemap.BaseLayers.Add(Cad_Layer);
        }
        public void Load_OpenStreetMap()
        {
            MainViewModel.SetBBox();
            MyMapView.Map = new Map(Basemap.CreateOpenStreetMap());
            MyMapView.SetViewpointCenterAsync(MainViewModel.CenterPoint);
            MainViewModel.SetBBox();
        }
        #endregion
        private void ToWGS84()
        {
            Coord = CoordinateFormatter.ToLatitudeLongitude(SelectedLocation, LatitudeLongitudeFormat.DecimalDegrees, 6);
        }

        public async Task CreatePictureMarker(GraphicsOverlay overlay)
        {
            overlay.Graphics.Clear();
            #region AddMark
            //var currentAssembly = Assembly.GetExecutingAssembly();
            //var resourceStream = currentAssembly.GetManifestResourceStream(
            //    "Doss.Resources.marker.png");
            //PictureMarkerSymbol pinSymbol = await PictureMarkerSymbol.CreateAsync(resourceStream);
            //pinSymbol.Width = 40;
            //pinSymbol.Height = 40;
            //MapPoint pinPoint = new MapPoint(SelectedLocation.X, SelectedLocation.Y, SpatialReferences.WebMercator);
            //Graphic pinGraphic = new Graphic(pinPoint, pinSymbol);
            //overlay.Graphics.Add(pinGraphic);
            #endregion
            var resourceStream = GetStreamFromUrl(MainViewModel.Img_Uri.ToString());
            PictureMarkerSymbol pinSymbol = await PictureMarkerSymbol.CreateAsync(resourceStream);
            pinSymbol.Width = MainViewModel.MyWindow.GridMap.ActualWidth;
            pinSymbol.Height = MainViewModel.MyWindow.GridMap.ActualHeight;
            MapPoint pinPoint = MainViewModel.CenterPoint;
            Graphic pinGraphic = new Graphic(pinPoint, pinSymbol);
            overlay.Graphics.Add(pinGraphic);
        }

        private MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;

            ms = null;

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {
                //forbidden, proxy issues, file not found (404) etc
            }

            return ms;
        }

        #region PropertyChanged

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
        #endregion
    }
}

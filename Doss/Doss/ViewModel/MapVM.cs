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
using Microsoft.Win32;
using System.Windows.Threading;
using Emgu.CV;
using Emgu.CV.Structure;

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
        private ImageWork ImageWorks;

        private Image<Gray, byte> GrayImagePKKwithBorder;
        private GraphicsOverlay _OverLayforBorder;


        #region prop
        public Map Map {get { return _map; }set { _map = value; OnPropertyChanged(); }}
        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; OnPropertyChanged(); } }
        public MapPoint SelectedLocation { get { return _SelectedLocation; } set { _SelectedLocation = value;OnPropertyChanged(); } }
        public GraphicsOverlay OverLay { get { return _OverLay; } set { _OverLay = value;OnPropertyChanged(); } }
        public GraphicsOverlay OverLayforBorder { get { return _OverLayforBorder; } set { _OverLayforBorder = value; OnPropertyChanged(); } }
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
            OverLayforBorder = new GraphicsOverlay();
            SelectedPlaceCoord = new PlaceCoord();
            GetPlaceProp = new GetPlace();
            MyMapView.GraphicsOverlays.Add(OverLay);
            MyMapView.GraphicsOverlays.Add(OverLayforBorder);
        }


        public void SetLocation(object sender, GeoViewInputEventArgs e)
        {
            SelectedLocation = e.Location;
            new Thread(() => SetLocationThread()).Start();

        }

        private void SetLocationThread()
        {
            
            ToWGS84();
            SelectedPlaceCoord = GetPlaceProp.GetPlaceCoordMethod(Coord.Substring(0, 9), Coord.Substring(12, 9));
            _Place = GetPlaceProp.Get_Place(SelectedPlaceCoord.Features[0].Attrs.Id);
            MainViewModel.UpdateInfo();
            MainViewModel.Img_Uri = MainViewModel.UpdateImgUri(MainViewModel.BBox_Left_Top, MainViewModel.BBox_Right_Bottom, MainViewModel.MapViewModel._Place, ((int)MainViewModel.MyWindow.GridMap.ActualWidth).ToString(), ((int)MainViewModel.MyWindow.GridMap.ActualHeight).ToString());
            CreatePictureMarker(OverLay, MainViewModel.Img_Uri).Wait();
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

        public async Task CreatePictureMarker(GraphicsOverlay overlay, Uri _uri)
        {
            overlay.Graphics.Clear();
            var resourceStream = GetStreamFromUrl(_uri.ToString());
            MainViewModel.SelectedPlace_Bitmap = CreateBitMap_FromUri(_uri);
            PictureMarkerSymbol pinSymbol = await PictureMarkerSymbol.CreateAsync(resourceStream);
            pinSymbol.Width = MainViewModel.MyWindow.GridMap.ActualWidth;
            pinSymbol.Height = MainViewModel.MyWindow.GridMap.ActualHeight;
            MapPoint pinPoint = MainViewModel.CenterPoint;
            Graphic pinGraphic = new Graphic(pinPoint, pinSymbol);
            overlay.Graphics.Add(pinGraphic);
            overlay.Graphics[0].ZIndex = 2;
            overlay.Opacity = 0.5;
        }
        public async Task CreatePictureBorder(GraphicsOverlay overlay, Uri _uri)
        {
            
            var resourceStream = GetStreamFromUrl(_uri.ToString());
            CreateBitMap_FromUri(_uri);
            ImageWorks =  new ImageWork(MainViewModel.SelectedPlace_Bitmap, MainViewModel.Scale,Math.Abs(MainViewModel._BorderValue));
            byte[] border_Img = await ImageWorks.CreateImg();
            PictureMarkerSymbol pinSymbol =  new PictureMarkerSymbol(new RuntimeImage(border_Img));
            pinSymbol.Width = MainViewModel.MyWindow.GridMap.ActualWidth;
            pinSymbol.Height = MainViewModel.MyWindow.GridMap.ActualHeight;
            MapPoint pinPoint = MyMapView.ScreenToLocation(ImageWorks.CenterPlace);
            Graphic pinGraphic = new Graphic(pinPoint, pinSymbol);
            overlay.Graphics.Add(pinGraphic);
            overlay.Graphics[1].ZIndex = 1;
            overlay.Opacity = 0.5;
        }

        public List<Place> MakeImageForReseach()
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog()
            { FileName = System.AppDomain.CurrentDomain.BaseDirectory + @"PKK.png" };
            GrayImagePKKwithBorder = ImageWorks.WorkWithCad(_openFileDialog);
            
            return ReseachImage();
        }

        private  List<Place> ReseachImage()
        {
            List<Place> ls = new List<Place>();
            DatePoint BlackPoint = ImageWorks.FindBlackPoint(GrayImagePKKwithBorder);
            while (BlackPoint._x != -1)
            {
                var location = MyMapView.ScreenToLocation(new Point(BlackPoint._x, BlackPoint._y));
                var coord = CoordinateFormatter.ToLatitudeLongitude(location, LatitudeLongitudeFormat.DecimalDegrees, 6);
                var _placeCoord = GetPlaceProp.GetPlaceCoordMethod(coord.Substring(0, 9), coord.Substring(12, 9));
                int cnt = _placeCoord.Features[0].Attrs.Cn.Count(c => c == ':');
                if (_placeCoord.Features[0].Attrs.Address != null)
                {
                    var place1 = GetPlaceProp.Get_Place(_placeCoord.Features[0].Attrs.Id);
                    ls.Add(place1);
                }
                LineFill(BlackPoint._x, BlackPoint._y);
                BlackPoint = ImageWorks.FindBlackPoint(GrayImagePKKwithBorder);
            }
            ls = ls.GroupBy(s => s.Feature.Attrs.Cn).Select(g => g.First()).ToList();
            return ls;
        }

        void LineFill(int x, int y)
        {
            
            if (x < GrayImagePKKwithBorder.Width && x > 0 && y < GrayImagePKKwithBorder.Height && y > 0 && GrayImagePKKwithBorder.Data[y, x, 0] != 255)
            {

                List<DatePoint> points = new List<DatePoint>();
                for (int i = x; i > 0; i--)
                {
                    if (GrayImagePKKwithBorder.Data[y, i, 0] != 255)
                    {
                        GrayImagePKKwithBorder.Data[y, i, 0] = 255;
                        points.Add(new DatePoint(i, y, 0));
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = x + 1; i < GrayImagePKKwithBorder.Width; i++)
                {
                    if (GrayImagePKKwithBorder.Data[y, i, 0] != 255)
                    {
                        GrayImagePKKwithBorder.Data[y, i, 0] = 255;
                        points.Add(new DatePoint(i, y, 0));
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (var item in points)
                {
                    LineFill(item._x, item._y + 1);
                    LineFill(item._x, item._y - 1);
                }
            }
        }
        private System.Drawing.Bitmap CreateBitMap_FromUri(Uri u)
        {
            System.Net.WebRequest request =
            System.Net.WebRequest.Create(u.ToString());
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream =
            response.GetResponseStream();
            var result = new System.Drawing.Bitmap(responseStream);
            return result;
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
        //public async Task CreateMarker(GraphicsOverlay overlay, MapPoint _MapPoint)
        //{
        //    //маркер на карте
        //    //overlay.Graphics.Clear();

        //    #region AddMark
        //    var currentAssembly = Assembly.GetExecutingAssembly();
        //    var resourceStream = currentAssembly.GetManifestResourceStream(
        //        "Doss.Resources.marker.png");
        //    PictureMarkerSymbol pinSymbol = await PictureMarkerSymbol.CreateAsync(resourceStream);
        //    pinSymbol.Width = 40;
        //    pinSymbol.Height = 40;
        //    Graphic pinGraphic = new Graphic(_MapPoint, pinSymbol);
        //    overlay.Graphics.Add(pinGraphic);
        //    #endregion
        //}
    }
}

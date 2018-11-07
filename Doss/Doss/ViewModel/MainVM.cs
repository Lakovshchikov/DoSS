using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doss.Model;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Doss.Model2;
using Doss.Model_Forms_Place;
using Doss.Model_Place_Categories;
using Doss.Model_Type_Of_Use;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Data;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Input;
using System.Net;

namespace Doss.ViewModel
{
    class MainVM : baseVM, INotifyPropertyChanged
    {

        private Map _Map;
        private MapVM _MapVM;
        private MapView _MyMapView;
        private Place _SelectedPlace;
        private Point _Left_Top;
        private Point _Right_Bottom;
        private MainWindow _Window;
        private MapPoint _BBox;
        private MapPoint _BBox_Left_Top;
        private MapPoint _BBox_Right_Bottom;
        private Uri _Img_Uri;
        private Point _CenterPoint;
        private MapPoint _CentMapPoint;

        private MapPoint _mapPointToScale;
        private double _scale;
        private bool _borderIsVisible;

        private TypesOf_Use _types;
        private Place_Categories _categories;
        private Form_Place _forms;

        private List<Place> FoundPlaces;

        // Bitmap для границ
        private System.Drawing.Bitmap _SelectedPlace_Bitmap;

        [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern int GdipGetDpiX(HandleRef graphics, float[] dpi);

        [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern int GdipGetDpiY(HandleRef graphics, float[] dpi);



        #region Info_about_place
        private string _type_LandPlot;
        private string _сad_num_LandPlot;
        private string _cad_quarter_LandPlot;
        private string _statys_LandPlot;
        private string _adress_LandPlot;
        private string _categ_LandPlot;
        private string _form_LandPlot;
        private string _cost_LandPlot;
        private string _permittedUse_LandPlot;
        private string _doc_LandPlot;
        private string _сadEng_LandPlot;
        private string _startDate_LandPlot;
        private string _changeDate_LandPlot;
        private string _loadingDate_LandPlot;
        private string _areaValue;
        #endregion

        #region prop
        public MapPoint CenterPoint { get { return _CentMapPoint; } set { _CentMapPoint = value; } }
        public Map Map { get { return _Map; } set { _Map = value; OnPropertyChanged(); } }
        public MapVM MapViewModel { get { return _MapVM; } set { _MapVM = value; } }
        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; } }
        public Place SelectedPlace { get { return _SelectedPlace; } set { MapViewModel._Place = value;OnPropertyChanged(); } }
        public Point Left_Top { get { return _Left_Top; } set { _Left_Top = value; } }
        public Point Right_Bottom { get { return _Right_Bottom; } set { _Right_Bottom = value; } }
        public MainWindow MyWindow { get { return _Window; } set { _Window = value; } }
        public MapPoint BBox_Left_Top { get { return _BBox_Left_Top; } set { _BBox_Left_Top = value; } }
        public MapPoint BBox_Right_Bottom { get { return _BBox_Right_Bottom; } set { _BBox_Right_Bottom = value; } }
        public Uri Img_Uri { get { return _Img_Uri; } set { _Img_Uri = value; OnPropertyChanged(); } }
        public System.Drawing.Bitmap SelectedPlace_Bitmap { get { return _SelectedPlace_Bitmap; } set { _SelectedPlace_Bitmap = value; OnPropertyChanged(); } }
        public double Scale { get { return _scale; } set { _scale = value; OnPropertyChanged(); } }
        #region Info_about_place_prop
        public string Type_LandPlot { get { return _type_LandPlot; } set { _type_LandPlot = value; OnPropertyChanged(); } }
        public string Cad_Num_LandPlot { get {  return _сad_num_LandPlot; } set { _сad_num_LandPlot = value; OnPropertyChanged(); } }
        public string Cad_quarter_LandPlot { get { return _cad_quarter_LandPlot; } set { _cad_quarter_LandPlot = value; OnPropertyChanged(); } }
        public string Status_LandPlot { get { return _statys_LandPlot; } set { _statys_LandPlot = value; OnPropertyChanged(); } }
        public string Adress_LandPlot { get { return _adress_LandPlot; } set { _adress_LandPlot = value; OnPropertyChanged(); } }
        public string Categ_LandPlot { get { return _categ_LandPlot; } set { _categ_LandPlot = value; OnPropertyChanged(); } }
        public string Form_LandPlot { get { return _form_LandPlot; } set { _form_LandPlot = value;OnPropertyChanged(); } }
        public string Cost_LandPlot { get { return _cost_LandPlot; } set { _cost_LandPlot = value;OnPropertyChanged(); } }
        public string PermirredUse_LandPlot { get { return _permittedUse_LandPlot; } set { _permittedUse_LandPlot = value;OnPropertyChanged(); } }
        public string Doc_LandPlot { get { return _doc_LandPlot; } set { _doc_LandPlot = value;OnPropertyChanged(); } }
        public string CadEng_LandPlot { get { return _сadEng_LandPlot; } set { _сadEng_LandPlot = value; OnPropertyChanged(); } }
        public string StartDate_LandPlot { get { return _startDate_LandPlot; } set { _startDate_LandPlot = value;OnPropertyChanged(); } }
        public string ChangeDate_LandPlot { get { return _changeDate_LandPlot; } set { _changeDate_LandPlot=value;OnPropertyChanged(); } }
        public string LoadingDate_LandPlot { get { return _loadingDate_LandPlot; } set { _loadingDate_LandPlot = value;OnPropertyChanged(); } }
        public string AreaValue { get { return _areaValue; } set { _areaValue = value; OnPropertyChanged(); } }
        #endregion
        #endregion

        #region propdp

        #region SelectMap
        public bool CadMap
        {
            get { return (bool)GetValue(CadMapProperty); }
            set { SetValue(CadMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CadMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CadMapProperty =
            DependencyProperty.Register("CadMap", typeof(bool), typeof(MainVM), new PropertyMetadata(false));

        public bool StreetMap
        {
            get { return (bool)GetValue(StreetMapProperty); }
            set { SetValue(StreetMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StreetMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StreetMapProperty =
            DependencyProperty.Register("StreetMap", typeof(bool), typeof(MainVM), new PropertyMetadata(false));


        public bool OpStMap
        {
            get { return (bool)GetValue(OpStMapProperty); }
            set { SetValue(OpStMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpStMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpStMapProperty =
            DependencyProperty.Register("OpStMap", typeof(bool), typeof(MainVM), new PropertyMetadata(false));

        public bool SpaceMap
        {
            get { return (bool)GetValue(SpaceMapProperty); }
            set { SetValue(SpaceMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpaceMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpaceMapProperty =
            DependencyProperty.Register("SpaceMap", typeof(bool), typeof(MainVM), new PropertyMetadata(false));

        #endregion

        public string _AreaValue
        {
            get { return (string)GetValue(_AreaValueProperty); }
            set { SetValue(_AreaValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _AreaValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _AreaValueProperty =
            DependencyProperty.Register("_AreaValue", typeof(string), typeof(MainVM), new PropertyMetadata(""));

        public int _BorderValue
        {
            get { return (int)GetValue(_BorderValueProperty); }
            set { SetValue(_BorderValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _BorderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _BorderValueProperty =
            DependencyProperty.Register("_BorderValue", typeof(int), typeof(MainVM), new PropertyMetadata(0));

        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); StatusBarTextChange(); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(bool), typeof(MainVM), new PropertyMetadata(false));


        public string _StatusBarText
        {
            get { return (string)GetValue(_StatusBarTextProperty); }
            set { SetValue(_StatusBarTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _StatusBarText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _StatusBarTextProperty =
            DependencyProperty.Register("_StatusBarText", typeof(string), typeof(MainVM), new PropertyMetadata("Подключение к сервисам росреестра. Пожалуйста подождите"));

        public bool BorderIsVisible
        {
            get { return (bool)GetValue(BorderIsVisibleProperty); }
            set { SetValue(BorderIsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderIsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderIsVisibleProperty =
            DependencyProperty.Register("BorderIsVisible", typeof(bool), typeof(MainVM), new PropertyMetadata(false));

        public string AllPlaces
        {
            get { return (string)GetValue(AllPlacesProperty); }
            set { SetValue(AllPlacesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllPlaces.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllPlacesProperty =
            DependencyProperty.Register("AllPlaces", typeof(string), typeof(MainVM), new PropertyMetadata(""));










        #endregion



        public MainVM(Action ok, Action close, MapView MyMapViewFormMV, MainWindow window) : base(ok, close)
        {
            _MyMapView = MyMapViewFormMV;
            _MapVM = new MapVM(MyMapView,this);
            _Map = MyMapViewFormMV.Map;
            _Window = window;
            _MyMapView.GeoViewTapped += MapViewModel.SetLocation;
            _Window.Show();
            _MyMapView.NavigationCompleted += NavigationCompleted;
            _MyMapView.PreviewMouseWheel += MouseWhell;
            SetBBox();
            SetCadMapCommand = new Command(SetCadMap, () => true);
            SetStreetMapCommand = new Command(SetStreetMap, () => true);
            SetOpenStreetMapCommand = new Command(SetOpenStreetMap, () => true);
            SetSpaceMapCommand = new Command(SetSpaceMap, () => true);
            SetBorderCommand = new Command(SetBorder, () => true);
            HideBorderCommand = new Command(HideDorder, () => true);
            ReseachBorderCommand = new Command(ReseachBorderThread, () => true);
            CreateWordDocCommad = new Command(CreateWordDoc, () => true);
            new Thread(() => DownloadInfo()).Start();
            #region dpi
            //var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            //var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            //var dpiX = (int)dpiXProperty.GetValue(null, null);
            //var dpiY = (int)dpiYProperty.GetValue(null, null);
            #endregion

        }


        private void NavigationCompleted(object sender, EventArgs e)
        {
            GC.Collect();
            SetBBox();
            _Img_Uri= UpdateImgUri(BBox_Left_Top, BBox_Right_Bottom, MapViewModel._Place, ((int)_Window.GridMap.ActualWidth).ToString(), ((int)_Window.GridMap.ActualHeight).ToString());
            UpdateMapPointToScale();
            if (_MapVM.SelectedLocation != null)
            {
                _MapVM.CreatePictureMarker(_MapVM.OverLay,Img_Uri).Wait();
                if (BorderIsVisible)
                {
                    //new Thread(() => _MapVM.CreatePictureBorder(_MapVM.OverLay, Img_Uri)).Start();
                    _MapVM.CreatePictureBorder(_MapVM.OverLay, Img_Uri);
                }
                //Thread.Sleep(5000);
                //_MapVM.CreatePictureMarker(_MapVM.OverLay, _Img_Border_Uri).Wait();
               // _MapVM.CreateMarker(_MapVM.OverLay, CenterPoint).Wait();
            }
        }
        private void MouseWhell(object sender, MouseEventArgs e)
        {
            _MapVM.OverLay.Graphics.Clear();
        }
        public void SetBBox()
        {
            
            _Left_Top = _Window.GridMap.PointToScreen(new Point(0, 0));

            //Размер правой точки относительно экрана
            //_Right_Bottom = _Window.GridMap.PointToScreen(new Point(_Window.GridMap.ActualWidth, _Window.GridMap.ActualHeight));

            _Right_Bottom = new Point(_Window.GridMap.ActualWidth, _Window.GridMap.ActualHeight);
            _CenterPoint = new Point(_Right_Bottom.X / 2, _Right_Bottom.Y / 2);
            CenterPoint = _MyMapView.ScreenToLocation(_CenterPoint);
            _Left_Top.X = 0;
            _Left_Top.Y = 0;
            _BBox_Left_Top = _MyMapView.ScreenToLocation(_Left_Top);
            
            //if (_MapVM.SelectedLocation != null)
            //{
            //    //ResetBBPoint(UpdateMapPointToScale());
            //    UpdateMapPointToScale();
            //}
            _BBox_Right_Bottom = _MyMapView.ScreenToLocation(_Right_Bottom);
            #region Старые расчеты с кликом
            //_Left_Top.X = _Left_Top.X + _Left_Top.X * 0.005;
            //_Left_Top.Y = _Left_Top.Y + _Left_Top.Y * 0.005;
            //_Right_Bottom.X = _Right_Bottom.X - _Right_Bottom.X * 0.005;
            //_Right_Bottom.Y = _Right_Bottom.Y - _Right_Bottom.Y * 0.035;

            //_MyMapView.GeoViewTapped -= MapViewModel.SetLocation;
            //_MyMapView.GeoViewTapped += SetLocationFromMain;
            
            //LeftMouseClick((int)_Left_Top.X, (int)_Left_Top.Y);
            //_BBox_Left_Top = _BBox;
            //LeftMouseClick((int)_Right_Bottom.X, (int)_Right_Bottom.Y);
            //_BBox_Right_Bottom = _BBox;

            //_MyMapView.GeoViewTapped -= SetLocationFromMain;
            //_MyMapView.GeoViewTapped += MapViewModel.SetLocation;
            #endregion
        }
        public Uri UpdateImgUri(MapPoint LeftTop_Point, MapPoint RightBottom_Point, Place SelectedPlace,string size_h, string size_w)
        {
            try
            {
                #region Форматирование
                string Xmin = toStr(LeftTop_Point.X);
                string Xmax = toStr(RightBottom_Point.X);
                string Ymin = toStr(LeftTop_Point.Y);
                string Ymax = toStr(RightBottom_Point.Y);

                string okrug = SelectedPlace.Feature.Attrs.Okrug.ToString();
                string rayon = SelectedPlace.Feature.Attrs.Rayon.ToString().Substring(SelectedPlace.Feature.Attrs.Rayon.ToString().IndexOf(':') + 1, SelectedPlace.Feature.Attrs.Rayon.ToString().Length - SelectedPlace.Feature.Attrs.Rayon.ToString().IndexOf(':') - 1);
                string kvartal = SelectedPlace.Feature.Attrs.Kvartal.ToString().Substring(SelectedPlace.Feature.Attrs.Kvartal.ToString().LastIndexOf(':') + 1, SelectedPlace.Feature.Attrs.Kvartal.ToString().Length - SelectedPlace.Feature.Attrs.Kvartal.ToString().LastIndexOf(':') - 1);
                string id = SelectedPlace.Feature.Attrs.AnnoText.ToString();
                //string num2
                #endregion
                string url = string.Format("arcgis/rest/services/Cadastre/CadastreSelected/MapServer/export?dpi=96&transparent=true&format=png32&layers=show%3A6%2C7&bbox={0}%2C{1}%2C{2}%2C{3}&bboxSR=102100&imageSR=102100&size={12}%2C{13}&layerDefs=%7B%226%22%3A%22ID%20%3D%20%27{4}%3A{5}%3A{6}%3A{7}%27%22%2C%227%22%3A%22ID%20%3D%20%27{8}%3A{9}%3A{10}%3A{11}%27%22%7D&f=image", Xmin, Ymin, Xmax, Ymax, okrug, rayon, kvartal, id, okrug, rayon, kvartal, id, size_h, size_w);
                Uri result = new Uri("https://pkk5.rosreestr.ru/" + url);
                return result;
            }
            catch (Exception)
            {
                
                return null;
                //throw;
            }
        }      
        private string toStr(double a)
        {
            string st = "";
            foreach (var item in a.ToString())
            {
                if (item != ',')
                {
                    st += item;
                }
                else
                {
                    st += ".";
                }
            }
            return st;
        }
        public void UpdateInfo()
        {
            Type_LandPlot = "Земельный участок";

            Cad_Num_LandPlot = MapViewModel._Place.Feature.Attrs.Cn;
            Cad_quarter_LandPlot = MapViewModel._Place.Feature.Attrs.KvartalCn;

            //Статус
            if (MapViewModel._Place.Feature.Attrs.Statecd == "01") Status_LandPlot = "Ранее учтеный";
            if (MapViewModel._Place.Feature.Attrs.Statecd == "06") Status_LandPlot = "Учтеный";

            Adress_LandPlot = MapViewModel._Place.Feature.Attrs.Address;
            AreaValue = MapViewModel._Place.Feature.Attrs.AreaValue.ToString() + " м^2";

            //Категория земель
            for (int i = 0; i < _categories.Fields[4].Domain.CodedValues.Length; i++)
            {
                if (MapViewModel._Place.Feature.Attrs.CategoryType == _categories.Fields[4].Domain.CodedValues[i].Code.String) Categ_LandPlot = _categories.Fields[4].Domain.CodedValues[i].Name;
            }

            //Форма собственности
            for (int i = 0; i < _forms.Fields[4].Domain.CodedValues.Length; i++)
            {
                if (MapViewModel._Place.Feature.Attrs.Fp == _forms.Fields[4].Domain.CodedValues[i].Code.ToString()) Form_LandPlot = _forms.Fields[4].Domain.CodedValues[i].Name;
            }

            Cost_LandPlot = MapViewModel._Place.Feature.Attrs.CadCost + " руб.";

            //Разрешенное использование
            for (int i = 0; i < _types.Fields[4].Domain.CodedValues.Length; i++)
            {
                if (MapViewModel._Place.Feature.Attrs.UtilCode == _types.Fields[4].Domain.CodedValues[i].Code.String) PermirredUse_LandPlot = _types.Fields[4].Domain.CodedValues[i].Name;
            }

            Doc_LandPlot = MapViewModel._Place.Feature.Attrs.UtilByDoc;

            //Кадастровый инженер
            CadEng_LandPlot = "";


            StartDate_LandPlot = MapViewModel._Place.Feature.Attrs.DateCreate;
            ChangeDate_LandPlot = MapViewModel._Place.Feature.Attrs.CadRecordDate;
            LoadingDate_LandPlot = MapViewModel._Place.Feature.Attrs.Adate;
        }

        public Uri UpdetePKKImageUri(MapPoint LeftTop_Point, MapPoint RightBottom_Point, Place SelectedPlace, string size_h, string size_w)
        {
            try
            {
                #region Форматирование
                string Xmin = toStr(LeftTop_Point.X);
                string Xmax = toStr(RightBottom_Point.X);
                string Ymin = toStr(LeftTop_Point.Y);
                string Ymax = toStr(RightBottom_Point.Y);

                string okrug = SelectedPlace.Feature.Attrs.Okrug.ToString();
                string rayon = SelectedPlace.Feature.Attrs.Rayon.ToString().Substring(SelectedPlace.Feature.Attrs.Rayon.ToString().IndexOf(':') + 1, SelectedPlace.Feature.Attrs.Rayon.ToString().Length - SelectedPlace.Feature.Attrs.Rayon.ToString().IndexOf(':') - 1);
                string kvartal = SelectedPlace.Feature.Attrs.Kvartal.ToString().Substring(SelectedPlace.Feature.Attrs.Kvartal.ToString().LastIndexOf(':') + 1, SelectedPlace.Feature.Attrs.Kvartal.ToString().Length - SelectedPlace.Feature.Attrs.Kvartal.ToString().LastIndexOf(':') - 1);
                string id = SelectedPlace.Feature.Attrs.AnnoText.ToString();
                //string num2
                #endregion
                string url = string.Format("arcgis/rest/services/Cadastre/CadastreWMS/MapServer/export?dpi=96&transparent=true&format=png32&bbox={0}%2C{1}%2C{2}%2C{3}&size={4},{5}&bboxSR=102100&imageSR=102100&f=image", Xmin, Ymin, Xmax, Ymax, size_h, size_w);
                Uri result = new Uri("https://pkk5.rosreestr.ru/" + url);
                return result;
            }
            catch (Exception)
            {

                return null;
                //throw;
            }
        }

        public void UpdateMapPointToScale()
        {
            Point secondP = new Point(0, 1);
            _mapPointToScale = _MyMapView.ScreenToLocation(secondP);
            Scale = GeometryEngine.Distance(_BBox_Left_Top, _mapPointToScale) / 1.735;
            //return a;
        }
        public void DownloadInfo()
        {
            Const_Request CR = new Const_Request();
            _forms = CR.GetRequestFormPlaceMethod();
            _categories = CR.GetRequestPlaceCategoriesMethod();
            _types = CR.GetRequestTypesOf_UseMethod();
            if (_forms != null && _categories != null && _types !=null)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { Status = true; }));
            }
            
        }

        #region Mouse_Click
        public void SetLocationFromMain(object sender, GeoViewInputEventArgs e)
        {
            _BBox = e.Location;
        }
        


        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        //This simulates a left mouse click
        public static void LeftMouseClick(int X, int Y)
        {
            SetCursorPos(X, Y);
            
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        #endregion

        #region SelectMap

        public void SetCadMap()
        {
            if (!CadMap)
            {
                int a = _MyMapView.Map.Basemap.BaseLayers.Count - 1;
                _MyMapView.Map.Basemap.BaseLayers.RemoveAt(a);
                CadMap = false;
            }
            else
            {
                CadMap = true;
                _MapVM.Load_Cad();
            }
        }
        public void SetStreetMap()
        {

            SpaceMap = false;
            StreetMap = true;
            OpStMap = false;
            _MapVM.Load_StreetMap();
            if (CadMap)
            {
                _MapVM.Load_Cad();
            }
        }
        public void SetOpenStreetMap()
        {
            SpaceMap = false;
            StreetMap = false;
            OpStMap = true;
            
            _MapVM.Load_OpenStreetMap();
            if (CadMap)
            {
                _MapVM.Load_Cad();
            }
        }
        public void SetSpaceMap()
        {
           
            SpaceMap = true;
            StreetMap = false;
            OpStMap = false;
           
            _MapVM.Load_ImageryWithLabels_Layer();
            if (CadMap)
            {
                _MapVM.Load_Cad();
            }
        }
        #endregion

        private void SetBorder()
        {
            //new Thread(() => _MapVM.CreatePictureBorder(_MapVM.OverLay, Img_Uri)).Start();
            _MapVM.CreatePictureBorder(_MapVM.OverLay, Img_Uri);
            BorderIsVisible = true;;
           


        }
        private void HideDorder()
        {
            try
            {
                _MapVM.OverLay.Graphics.Remove(_MapVM.OverLay.Graphics[1]);
                BorderIsVisible = false;
            }
            catch (Exception)
            {
            }
            
        }

        private void ReseachBorderThread()
        {
            _StatusBarText = "Идет анализ, пожалуйста подождите";
            Uri uriPKK = UpdetePKKImageUri(BBox_Left_Top, BBox_Right_Bottom, MapViewModel._Place, ((int)_Window.GridMap.ActualWidth).ToString(), ((int)_Window.GridMap.ActualHeight).ToString());
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(uriPKK, System.AppDomain.CurrentDomain.BaseDirectory + @"PKK.png");
            }
            FoundPlaces = MapViewModel.MakeImageForReseach();
            foreach (var item in FoundPlaces)
            {
                AllPlaces += item.Feature.Attrs.Cn + " ";
            }
            _StatusBarText = "Анализ завершен. Рабочая область готова для использования";
        }

        private void ReseachBorder()
        {
            new Thread(() => ReseachBorderThread()).Start();
        }

        private void CreateWordDoc()
        {
            double screenLeft = _Window.GridMap.PointToScreen(new Point(0, 0)).X;
            double screenTop = _Window.GridMap.PointToScreen(new Point(0, 0)).Y;
            double screenWidth = _Window.GridMap.PointToScreen(new Point(_Window.GridMap.ActualWidth, _Window.GridMap.ActualHeight)).X - screenLeft;

            double screenHeight = _Window.GridMap.PointToScreen(new Point(_Window.GridMap.ActualWidth, _Window.GridMap.ActualHeight)).Y - screenTop;

            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("pictureforDoc.png");
                }

            }
            WorkWithDoc Doc = new WorkWithDoc(FoundPlaces,_categories);
        }

        #region command
        public Command SetCadMapCommand { get; set; }
        public Command SetStreetMapCommand { get; set; }
        public Command SetOpenStreetMapCommand { get; set; }
        public Command SetSpaceMapCommand { get; set; }
        public Command SetBorderCommand { get; set; }
        public Command HideBorderCommand { get; set; }
        public Command ReseachBorderCommand { get; set; }
        public Command CreateWordDocCommad { get; set; }
        #endregion

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void StatusBarTextChange()
        {
            if (Status == true)
            {
                _StatusBarText = "Загрузка завершена. Рабочая область готова для использования";
            }
            else
            {
                _StatusBarText = "Подключение к сервисам росреестра. Пожалуйста подождите";
            }
        }
    }
}

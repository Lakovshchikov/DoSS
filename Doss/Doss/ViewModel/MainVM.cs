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
using Doss.Model2;

namespace Doss.ViewModel
{
    class MainVM : baseVM, INotifyPropertyChanged
    {

        private Map _map;
        private MapVM _MapVM;
        private MapView _MyMapView;
        private Place _SelectedPlace;
        private bool _IsEnabled_Cad_Map;
        private bool _IsEnabled_Street_Map;
        private bool _IsEnabled_Space_Map;

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
        #endregion

        #region prop
        public Map Map { get { return _map; } set { _map = value; OnPropertyChanged(); } }
        public MapVM MapViewModel { get { return _MapVM; } set { _MapVM = value; } }
        public MapView MyMapView { get { return _MyMapView; } set { _MyMapView = value; } }
        public Place SelectedPlace { get { return _SelectedPlace; } set { MapViewModel._Place = value;OnPropertyChanged(); } }
        public bool IsEnabled_Cad_Map { get { return _IsEnabled_Cad_Map; } set { _IsEnabled_Cad_Map = value;OnPropertyChanged(); SetCadMap(); } }
        public bool IsEnabled_Street_Map { get { return _IsEnabled_Street_Map; } set { _IsEnabled_Street_Map = value; OnPropertyChanged(); SetStreetMap(); } }
        public bool IsEnabled_Space_Map { get { return _IsEnabled_Space_Map; } set { _IsEnabled_Space_Map = value; OnPropertyChanged(); SetSpaseMap(); } }

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
        #endregion
        #endregion



        public MainVM(Action ok, Action close, MapView MyMapViewFormMV) : base(ok, close)
        {
            MyMapView = MyMapViewFormMV;
            MapViewModel = new MapVM(MyMapView,this);
            Map = MapViewModel.Map;
            MyMapView.GeoViewTapped += MapViewModel.SetLocation;
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

            //Категория земель
            if (MapViewModel._Place.Feature.Attrs.CategoryType == "003001000000") Categ_LandPlot = "Земли сельско-хозяйственного назначения";
            if (MapViewModel._Place.Feature.Attrs.CategoryType == "003002000000") Categ_LandPlot = "Земли насельнных пунктов";

            //Форма собственности
            if (MapViewModel._Place.Feature.Attrs.Fp == "100") Form_LandPlot = "Частная собственность";
            if (MapViewModel._Place.Feature.Attrs.Fp == "200") Form_LandPlot = "Собственность публично-правовых образований";

            Cost_LandPlot = MapViewModel._Place.Feature.Attrs.CadCost + " руб.";

            //Разрешенное использование
            PermirredUse_LandPlot = "";

            Doc_LandPlot = MapViewModel._Place.Feature.Attrs.UtilByDoc;

            //Кадастровый инженер
            CadEng_LandPlot = "";


            StartDate_LandPlot = MapViewModel._Place.Feature.Attrs.DateCreate;
            ChangeDate_LandPlot = MapViewModel._Place.Feature.Attrs.CadRecordDate;
            LoadingDate_LandPlot = MapViewModel._Place.Feature.Attrs.Adate;
        }

        public void SetCadMap()
        {
            if (IsEnabled_Cad_Map)
                MapViewModel.IsEnabled_Cad_Map = true;
            else
                MapViewModel.IsEnabled_Cad_Map = false;
        }
        public void SetStreetMap()
        {
            if (IsEnabled_Street_Map)
                MapViewModel.IsEnabled_Street_Map = true;
            else
                MapViewModel.IsEnabled_Street_Map = false;
        }
        public void SetSpaseMap()
        {
            if (IsEnabled_Space_Map)
                MapViewModel.IsEnabled_Space_Map = true;
            else
                MapViewModel.IsEnabled_Space_Map = false;
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

using Doss.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doss
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainVM m; 
        public MainWindow()
        {
            InitializeComponent();
            m = new MainVM(() => this.Close(), null, MyMapView, this);
            DataContext = m;
            var dpi = GetDpi();

        }
        #region 1_dpi
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private static float GetDpi()
        {
            IntPtr desktopWnd = IntPtr.Zero;
            IntPtr dc = GetDC(desktopWnd);
            var dpi = 100f;
            const int LOGPIXELSX = 88;
            try
            {
                dpi = GetDeviceCaps(dc, LOGPIXELSX);
            }
            finally
            {
                ReleaseDC(desktopWnd, dc);
            }
            return dpi / 96f;
        }
        #endregion;

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var windowPosition = Mouse.GetPosition(this);
            var screenPosition = this.PointToScreen(windowPosition);
            a.Text = string.Format("{0} --- {1}", windowPosition, screenPosition);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m.SetBBox(); 
        }
    }
}

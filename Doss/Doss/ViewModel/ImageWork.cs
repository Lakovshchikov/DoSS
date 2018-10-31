using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Doss.Model;

namespace Doss.ViewModel
{
    class ImageWork:INotifyPropertyChanged
    {
        private Image<Bgr, byte> _ImageIn;
        private Image<Bgr, byte> _ImageOut;
        private Image<Bgr, byte> _ImagePKK;
        private Image<Bgr, byte> _ImageWithBorder;
        private Image<Gray, byte> _ImageForFill;

        private BitmapSource _ImageInBitMap;
        private BitmapSource _ImageOutBitMap;
        private BitmapSource _ImageOutBitMapToPKK;
        private BitmapSource _ImagePKKBitMapSource;


        private int border;
        private double scale;

        Image<Gray, byte> BigImg;
        private List<DatePoint> LisiColorPoints = new List<DatePoint>();
        private List<DatePoint> ListBorderPonts = new List<DatePoint>();
        #region prop
        public Image<Bgr, byte> ImageIn { get { return _ImageIn; } set { _ImageIn = value; } }
        public Image<Bgr, byte> ImageOut { get { return _ImageOut; } set { _ImageOut = value; } }
        public BitmapSource ImageInBitMap { get { return _ImageInBitMap; } set { _ImageInBitMap = value; } }
        public BitmapSource ImageOutBitMap { get { return _ImageOutBitMap; } set { _ImageOutBitMap = value; } }
        public BitmapSource ImageOutBitMapToPKK { get { return _ImageOutBitMapToPKK; } set { _ImageOutBitMapToPKK = value; } }
        public BitmapSource ImagePKKBitMapSource { get { return _ImagePKKBitMapSource; } set { _ImagePKKBitMapSource = value; } }
        public System.Windows.Point CenterPlace { get; set; }

        #endregion
        public ImageWork(Bitmap input_img, double _scale, int _border)
        {
            scale = _scale;
            border = _border;
            ImageIn = new Image<Bgr, Byte>(input_img);
        }


        public async Task<byte[]> CreateImg()
        {
            //переменные для расчета центар входящего изображения
            int ind_left = 100000;
            int ind_right = -1;
            int ind_top = -1;
            int ind_bot = -1;
            //создание зеленого изображения участка
            ImageOut = ImageIn.Convert<Bgr, byte>().ThresholdBinary(new Bgr(0, 0, 0), new Bgr(120, 255, 0));
            //создание выходных изображений(чб и цветное)
            BigImg = new Image<Gray, byte>((ImageIn.Width + (int)((border / scale) * 4)), (ImageIn.Height + (int)((border / scale) * 4)));
            Image<Bgr, byte> BigImg_Color = new Image<Bgr, byte>((ImageIn.Width + (int)((border / scale) * 4)), (ImageIn.Height + (int)((border / scale) * 4)));

            //расчет центральной точки
            #region Верхний индекс
            for (int i = 0; i < ImageOut.Height; i++)
            {
                for (int j = 0; j < ImageOut.Width; j++)
                {
                    if (ImageOut.Data[i, j, 1] != 0)
                    {
                        ind_top = i;
                        break;
                    }
                }
                if (ind_top == i)
                {
                    break;
                }
            }
            #endregion
            #region Нижний индекс
            for (int i = 0; i < ImageOut.Height; i++)
            {
                for (int j = 0; j < ImageOut.Width; j++)
                {
                    if (ImageOut.Data[i, j, 1] != 0)
                    {
                        if (ind_bot <= i)
                        {
                            ind_bot = i;
                        }
                    }
                }
            }
            #endregion
            #region Левый индекс
            for (int i = 0; i < ImageOut.Height; i++)
            {
                for (int j = 0; j < ImageOut.Width; j++)
                {
                    if (ImageOut.Data[i, j, 1] != 0)
                    {
                        if (ind_left > j)
                        {
                            ind_left = j;
                        }
                    }
                }
            }
            #endregion
            #region Правый индекс
            for (int i = 0; i < ImageOut.Height; i++)
            {
                for (int j = 0; j < ImageOut.Width; j++)
                {
                    if (ImageOut.Data[i, j, 1] != 0)
                    {
                        if (ind_right < j)
                        {
                            ind_right = j;
                        }
                    }
                }
            }
            #endregion

            System.Drawing.Point center = new System.Drawing.Point(ind_left + (ind_right - ind_left) / 2, ind_top + (ind_bot - ind_top) / 2);
            //совмещение входящего изображения и выходного ЧБ
            BigImg = SummImg(BigImg, ImageIn);
            //отрисовка границ СЗЗ
            BigImg = DrawBorder(BigImg);
            //рисунок границ СЗЗ совмещается с выходныс изображением (цветное)
            BigImg_Color = SummImg(BigImg_Color, BigImg);

            //Image<> to Bitmapsource
            ImageOutBitMap = ToBitmapSource(BigImg_Color);

            //Bitmapsource to Bitmap
            Bitmap result_img = GetBitmap(ImageOutBitMap);
            //Прозрачность
            var color = System.Drawing.Color.Black;
            result_img.MakeTransparent(color);
           // result_img.Save("result.png");

            ImageConverter converter = new ImageConverter();
            byte[] res_img = (byte[])converter.ConvertTo(result_img, typeof(byte[]));
            return res_img;
        }

        private Image<Gray, byte> SummImg(Image<Gray, byte> first, Image<Bgr, byte> second)
        {
            System.Drawing.Point centerFirst = new System.Drawing.Point(first.Width / 2, first.Height / 2);
            int ind_left = 100000;
            int ind_right = -1;
            int ind_top = -1;
            int ind_bot = -1;
            List<DatePoint> yellowPoints = new List<DatePoint>();

            #region Верхний индекс
            for (int i = 0; i < second.Height; i++)
            {
                for (int j = 0; j < second.Width; j++)
                {
                    if (second.Data[i, j, 1] == 255)
                    {
                        ind_top = i;
                        break;
                    }
                }
                if (ind_top == i)
                {
                    break;
                }
            }
            #endregion
            #region Нижний индекс
            for (int i = 0; i < second.Height; i++)
            {
                for (int j = 0; j < second.Width; j++)
                {
                    if (second.Data[i, j, 1] == 255)
                    {
                        if (ind_bot <= i)
                        {
                            ind_bot = i;
                        }
                    }
                }
            }
            #endregion
            #region Левый индекс
            for (int i = 0; i < second.Height; i++)
            {
                for (int j = 0; j < second.Width; j++)
                {
                    if (second.Data[i, j, 1] == 255)
                    {
                        if (ind_left > j)
                        {
                            ind_left = j;
                        }
                    }
                }
            }
            #endregion
            #region Правый индекс
            for (int i = 0; i < second.Height; i++)
            {
                for (int j = 0; j < second.Width; j++)
                {
                    if (second.Data[i, j, 1] == 255)
                    {
                        if (ind_right < j)
                        {
                            ind_right = j;
                        }
                    }
                }
            }
            #endregion

            System.Drawing.Point centerSecond = new System.Drawing.Point(ind_left + (ind_right - ind_left) / 2, ind_top + (ind_bot - ind_top) / 2);
            //вектор смещения всех точек
            CenterPlace = new System.Windows.Point(ind_left + (ind_right - ind_left) / 2, ind_top + (ind_bot - ind_top) / 2);
            System.Drawing.Point vector = new System.Drawing.Point(centerFirst.Y - centerSecond.Y, centerFirst.X - centerSecond.X);





            #region нахождение всех точек желтого цвета
            for (int i = 0; i < second.Height; i++)
            {
                for (int j = 0; j < second.Width; j++)
                {
                        if (second.Data[i, j, 1] != 0)
                        {
                            yellowPoints.Add(new DatePoint(i, j, 0));
                        }

                }
            }
            #endregion
            #region Слияние рисунков
            foreach (var item in yellowPoints)
            {
                first.Data[(int)vector.X + item._x, (int)vector.Y + item._y, 0] = 255;
            }
            #endregion
            return first;

        }

        private Image<Bgr, byte> SummImg(Image<Bgr, byte> Color_i, Image<Gray, byte> BW_i)
        {
            List<DatePoint> yellowPoints = new List<DatePoint>();
            #region нахождение всех точек бедого цвета
            for (int i = 0; i < BW_i.Height; i++)
            {
                for (int j = 0; j < BW_i.Width; j++)
                {
                    if (BW_i.Data[i, j, 0] != 0)
                    {
                        yellowPoints.Add(new DatePoint(i, j, 0));
                    }
                }
            }
            #endregion
            #region Слияние рисунков
            foreach (var item in yellowPoints)
            {
                Color_i.Data[item._x, item._y, 0] = 120;
                Color_i.Data[item._x, item._y, 1] = 255;
            }
            #endregion
            #region Обрезка по цветному изображению
            //int ind_left = 100000;
            //int ind_right = -1;
            //int ind_top = -1;
            //int ind_bot = -1;
            ////расчет центральной точки
            //#region Верхний индекс
            //for (int i = 0; i < Color_i.Height; i++)
            //{
            //    for (int j = 0; j < Color_i.Width; j++)
            //    {
            //        if (Color_i.Data[i, j, 0] != 0)
            //        {
            //            ind_top = i;
            //            break;
            //        }
            //    }
            //    if (ind_top == i)
            //    {
            //        break;
            //    }
            //}
            //#endregion
            //#region Нижний индекс
            //for (int i = 0; i < Color_i.Height; i++)
            //{
            //    for (int j = 0; j < Color_i.Width; j++)
            //    {
            //        if (Color_i.Data[i, j, 0] != 0)
            //        {
            //            if (ind_bot <= i)
            //            {
            //                ind_bot = i;
            //            }
            //        }
            //    }
            //}
            //#endregion
            //#region Левый индекс
            //for (int i = 0; i < Color_i.Height; i++)
            //{
            //    for (int j = 0; j < Color_i.Width; j++)
            //    {
            //        if (Color_i.Data[i, j, 0] != 0)
            //        {
            //            if (ind_left > j)
            //            {
            //                ind_left = j;
            //            }
            //        }
            //    }
            //}
            //#endregion
            //#region Правый индекс
            //for (int i = 0; i < Color_i.Height; i++)
            //{
            //    for (int j = 0; j < Color_i.Width; j++)
            //    {
            //        if (Color_i.Data[i, j, 0] != 0)
            //        {
            //            if (ind_right < j)
            //            {
            //                ind_right = j;
            //            }
            //        }
            //    }
            //}
            //#endregion


            //yellowPoints.Clear();
            ////SaveClipboardImageToFile("img2.png", ToBitmapSource(Color_i));

            //Image<Bgr, byte> result_Img = new Image<Bgr, byte>(ind_right - ind_left + 1, ind_bot - ind_top + 1);
            //System.Drawing.Point vector = new System.Drawing.Point(ind_left, ind_top);

            //for (int i = 0; i < Color_i.Height; i++)
            //{
            //    for (int j = 0; j < Color_i.Width; j++)
            //    {
            //        if (Color_i.Data[i, j, 0] != 0)
            //        {
            //            yellowPoints.Add(new DatePoint(j, i, 0));
            //        }
            //    }
            //}

            //foreach (var item in yellowPoints)
            //{
            //    var a = item._y - vector.Y;
            //    var b = item._x - vector.X;
            //    result_Img.Data[a, b, 0] = 120;
            //    result_Img.Data[a, b, 1] = 255;
            //}
            #endregion
            #region Обрезка по размерам входящего изображения
            yellowPoints.Clear();
            int delta_x = (Color_i.Width - ImageIn.Width) / 2;
            int delta_y = (Color_i.Height - ImageIn.Height) / 2;
            for (int i = delta_y; i < Color_i.Height - delta_y; i++)
            {
                for (int j = delta_x; j < Color_i.Width - delta_x; j++)
                {
                    if (BW_i.Data[i, j, 0] != 0)
                    {
                        yellowPoints.Add(new DatePoint(j - delta_x, i - delta_x, 0));
                    }
                }
            }

            Image<Bgr, byte> result_Img = new Image<Bgr, byte>(ImageIn.Width+1, ImageIn.Height+1);
            foreach (var item in yellowPoints)
            {
                result_Img.Data[item._y, item._x, 0] = 120;
                result_Img.Data[item._y, item._x, 1] = 255;
            }
            #endregion
            return  result_Img;

        }

        private Image<Gray, byte> DrawBorder(Image<Gray, byte> img)
        {
            List<DatePoint> points = new List<DatePoint>();
            int pixS = (int)(border / scale);
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (img.Data[i, j, 0] == 255 && (img.Data[i + 1, j, 0] == 0 || img.Data[i - 1, j, 0] == 0 || img.Data[i, j + 1, 0] == 0 || img.Data[i, j - 1, 0] == 0))
                    {
                        points.Add(new DatePoint(j, i, 0));
                    }
                }
            }
            foreach (var item in points)
            {
                CvInvoke.Circle(img, new System.Drawing.Point(item._x, item._y), pixS, new MCvScalar(255), -1);
            }
            #region Первый прогон (верхняя строчка + левая диагональ для левой стороны
            //for (int i = 0; i < img.Height; i++)
            //{
            //    for (int j = 0; j < img.Width; j++)
            //    {
            //        if (img.Data[i, j, 0] == 255 && img.Data[i-1, j, 0] == 0)
            //        {
            //            //flagFistRow = true;
            //            //if (img.Data[i, j+1, 0] != 255)
            //            //{
            //            //    img.Data[i, j + pixS, 0] = 255;
            //            //}
            //            //if (img.Data[i, j - 1, 0] != 255)
            //            //{
            //            //    img.Data[i, j - pixS, 0] = 255;
            //            //}
            //            img.Data[i- pixS, j, 0] = 255;
            //            img.Data[i - pixS - 1, j, 0] = 255;
            //            img.Data[i - pixS - 2, j, 0] = 255;
            //            img.Data[i - pixS + 1, j, 0] = 255;
            //            img.Data[i - pixS + 2, j, 0] = 255;

            //            point = Calc_Left_DiagPoint(j,i,pixS_Diag);
            //            img.Data[point.X, point.Y, 0] = 255;
            //            img.Data[point.X+1, point.Y, 0] = 255;
            //            img.Data[point.X-1, point.Y, 0] = 255;
            //        }
            //    }
            //    //if (flagFistRow==true)
            //    //{
            //    //    break;
            //    //}
            //}
            #endregion
            return img;
        }

        public static void SaveClipboardImageToFile(string filePath, BitmapSource image)
        {

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
        public Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
        public BitmapSource Convert(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

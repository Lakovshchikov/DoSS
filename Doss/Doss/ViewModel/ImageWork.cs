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
using Microsoft.Win32;

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

        #region RGB const
        private int[] Rcolors = new int[] { 245, 168, 184,233, 184,234,198,211,223,253,255};
        private int[] Gcolors = new int[] { 238, 0, 105, 199, 105, 220, 146, 175, 199,253,255 };
        private int[] Bcolors = new int[] { 238, 0, 105, 199, 105, 220, 146, 175, 199, 253, 255 };
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
            _ImageWithBorder = BigImg_Color;

            //Image<> to Bitmapsource
            ImageOutBitMap = ToBitmapSource(BigImg_Color);

            //Bitmapsource to Bitmap
            Bitmap result_img = GetBitmap(ImageOutBitMap);
            //Прозрачность
            var color = System.Drawing.Color.Black;
            result_img.MakeTransparent(color);
            // result_img.Save("result.png");
            //WorkWithCad();
            ImageConverter converter = new ImageConverter();
            byte[] res_img = (byte[])converter.ConvertTo(result_img, typeof(byte[]));
            return res_img;

        }

        public Image<Gray,byte> WorkWithCad(OpenFileDialog _ofd)
        {
            _ImagePKK = new Image<Bgr, Byte>(_ofd.FileName);

            Image<Gray, byte> ResultImage;
            if (ImageIn != null)
            {
                ResultImage = MoveBorderToPlace();
            }
            else
            {
                ResultImage = new Image<Gray, byte>(_ImagePKK.Width, _ImagePKK.Height);
            }
            ResultImage = MovePKKtoPlace(ResultImage, _ImagePKK);
            ResultImage = WhiteAroundBorder(ResultImage);

            //_ImageForFill = ResultImage.Copy();
            //DatePoint BlackPoint = FindBlackPoint(_ImageForFill);
            //Fill(BlackPoint._x, BlackPoint._y);
            return ResultImage;

            //ImageOutBitMapToPKK = ToBitmapSource(_ImageForFill);
            //SaveClipboardImageToFile("img13.png", ImageOutBitMapToPKK);
            //RaisePropertyChanged("ImageOutBitMapToPKK");

        }

        #region Работа с ПКК

        

        public DatePoint FindBlackPoint(Image<Gray, byte> image)
        {
            DatePoint result = new DatePoint();
            try
            {
                result._x = -1;

                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        if (image.Data[i, j, 0] != 255)
                        {
                            result._x = j;
                            result._y = i;
                            result._z = j;
                            result._value = 0;
                            break;
                        }
                    }
                    if (result._x != -1)
                    {
                        break;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                result._x = -1;
                return result;
            }
           
        }
        private Image<Gray, byte> WhiteAroundBorder(Image<Gray, byte> image)
        {
            List<DatePoint> pointsToWhite = new List<DatePoint>();
            List<List<DatePoint>> pointsBorder = new List<List<DatePoint>>();
            #region Разбивка точек границ по линиям
            var list = ListBorderPonts.OrderBy(x => x._y).ThenBy(x => x._x);
            int BreakPoint = 0;
            int k = 0;
            ListBorderPonts = list.ToList();
            while (BreakPoint <= ListBorderPonts.Count - 1)
            {
                pointsBorder.Add(new List<DatePoint>());
                for (int i = BreakPoint; i <= ListBorderPonts.Count - 1; i++)
                {
                    if (i != ListBorderPonts.Count - 1)
                    {
                        if (ListBorderPonts[i]._y == ListBorderPonts[i + 1]._y)
                        {
                            pointsBorder[k].Add(ListBorderPonts[i]);
                        }
                        else
                        {
                            pointsBorder[k].Add(ListBorderPonts[i]);
                            BreakPoint = i + 1;
                            break;
                        }
                    }
                    else
                    {
                        pointsBorder[k].Add(ListBorderPonts[i]);
                        BreakPoint = i + 1;
                        break;
                    }
                }
                k++;
            }
            #endregion
            #region Белые точки вокруг
            //Точки сверху
            for (int i = 0; i < pointsBorder.First().First()._y; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    image.Data[i, j, 0] = 255;
                }
            }
            //Точки снизу
            for (int i = pointsBorder.Last().First()._y; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    image.Data[i, j, 0] = 255;
                }
            }
            //Точки слева
            int z = 0;
            for (int i = pointsBorder.First().First()._y; i < pointsBorder.Last().First()._y; i++)
            {
                for (int j = 0; j < pointsBorder[z].First()._x; j++)
                {
                    image.Data[i, j, 0] = 255;
                }
                z++;
            }
            //Точки справа
            z = 0;
            for (int i = pointsBorder.First().First()._y; i < pointsBorder.Last().First()._y; i++)
            {
                for (int j = pointsBorder[z].Last()._x; j < image.Width; j++)
                {
                    image.Data[i, j, 0] = 255;
                }
                z++;
            }
            #endregion
            return image;
        }

        private Image<Gray, byte> MoveBorderToPlace()
        {
            Image<Gray, byte> ImageInGray = ImageIn.Convert<Gray, byte>();
            Image<Gray, byte> ImageBorder = _ImageWithBorder.Convert<Gray, byte>();
            int ind_left = 100000;
            int ind_right = -1;
            int ind_top = -1;
            int ind_bot = -1;
            #region Верхний индекс
            for (int i = 0; i < ImageInGray.Height; i++)
            {
                for (int j = 0; j < ImageInGray.Width; j++)
                {
                    if (ImageInGray.Data[i, j, 0] != 0)
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
            for (int i = 0; i < ImageInGray.Height; i++)
            {
                for (int j = 0; j < ImageInGray.Width; j++)
                {
                    if (ImageInGray.Data[i, j, 0] != 0)
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
            for (int i = 0; i < ImageInGray.Height; i++)
            {
                for (int j = 0; j < ImageInGray.Width; j++)
                {
                    if (ImageInGray.Data[i, j, 0] != 0)
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
            for (int i = 0; i < ImageInGray.Height; i++)
            {
                for (int j = 0; j < ImageInGray.Width; j++)
                {
                    if (ImageInGray.Data[i, j, 0] != 0)
                    {
                        if (ind_right < j)
                        {
                            ind_right = j;
                        }
                    }
                }
            }
            #endregion

            System.Drawing.Point centerImageIn = new System.Drawing.Point(ind_left + (ind_right - ind_left) / 2, ind_top + (ind_bot - ind_top) / 2);
            System.Drawing.Point centerImageBorder = new System.Drawing.Point(_ImageWithBorder.Width / 2, _ImageWithBorder.Height / 2);
            System.Drawing.Point vector = new System.Drawing.Point(Math.Max(centerImageIn.X, centerImageBorder.X) - Math.Min(centerImageIn.X, centerImageBorder.X), Math.Max(centerImageIn.Y, centerImageBorder.Y) - Math.Min(centerImageIn.Y, centerImageBorder.Y));
            //все точки на границе СЗЗ

            List<DatePoint> pointsPlace = new List<DatePoint>();
            //Границы
            for (int i = 0; i < ImageBorder.Height; i++)
            {
                for (int j = 0; j < ImageBorder.Width; j++)
                {
                    if (ImageBorder.Data[i, j, 0] != 0 && (ImageBorder.Data[i + 1, j, 0] == 0 || ImageBorder.Data[i - 1, j, 0] == 0 || ImageBorder.Data[i, j + 1, 0] == 0 || ImageBorder.Data[i, j - 1, 0] == 0))
                    {
                        ListBorderPonts.Add(new DatePoint(j, i, 0));
                    }
                }
            }
            //Белый участок
            for (int i = 0; i < ImageInGray.Height; i++)
            {
                for (int j = 0; j < ImageInGray.Width; j++)
                {
                    if (ImageInGray.Data[i, j, 0] != 0)
                    {
                        pointsPlace.Add(new DatePoint(j, i, 0));
                    }
                }
            }
            //перенос в записимости от взаиморасположения центров
            List<DatePoint> _list_border_points = new List<DatePoint>();
            foreach (var item in ListBorderPonts)
            {
                _list_border_points.Add(item);
            }
            ListBorderPonts.Clear();
            foreach (var item in _list_border_points)
            {
                try
                {
                    if (centerImageBorder.X >= centerImageIn.X && centerImageBorder.Y >= centerImageIn.Y)
                    {
                        ImageInGray.Data[item._y - vector.Y, item._x - vector.X, 0] = 255;
                        ListBorderPonts.Add(new DatePoint(item._x - vector.X, item._y - vector.Y, 0));
                    }
                    else if (centerImageBorder.X >= centerImageIn.X && centerImageBorder.Y <= centerImageIn.Y)
                    {
                        ImageInGray.Data[item._y + vector.Y, item._x - vector.X, 0] = 255;
                        ListBorderPonts.Add(new DatePoint(item._x - vector.X, item._y + vector.Y, 0));
                    }
                    else if (centerImageBorder.X <= centerImageIn.X && centerImageBorder.Y >= centerImageIn.Y)
                    {
                        ImageInGray.Data[item._y - vector.Y, item._x + vector.X, 0] = 255;
                        ListBorderPonts.Add(new DatePoint(item._x + vector.X, item._y - vector.Y, 0));
                    }
                    else if (centerImageBorder.X <= centerImageIn.X && centerImageBorder.Y <= centerImageIn.Y)
                    {
                        ImageInGray.Data[item._y + vector.Y, item._x + vector.X, 0] = 255;
                        ListBorderPonts.Add(new DatePoint(item._x + vector.X, item._y + vector.Y, 0));
                    }
                }
                catch (Exception)
                {
                    continue;
                    throw;
                }

            }
            foreach (var item in pointsPlace)
            {
                ImageInGray.Data[item._y, item._x, 0] = 255;
            }
            return ImageInGray;
        }

        private Image<Gray, byte> MovePKKtoPlace(Image<Gray, byte> ImagePlace, Image<Bgr, byte> ImagePKK)
        {
            List<DatePoint> points = new List<DatePoint>();
            for (int i = 0; i < ImagePKK.Height; i++)
            {
                for (int j = 0; j < ImagePKK.Width; j++)
                {
                    if (i != 0 && i != ImagePKK.Height - 1 && j != 0 && j != ImagePKK.Width - 1)
                    {
                        if ((ImagePKK.Data[i, j, 2] > 0 && ImagePKK.Data[i, j, 0] < 200 && ImagePKK.Data[i, j, 1] < 200))
                        {
                            points.Add(new DatePoint(j, i, 0));
                        }
                        if ((ImagePKK.Data[i, j, 2] > 150 && ImagePKK.Data[i, j, 0] > 150 && ImagePKK.Data[i, j, 1] > 150))
                        {
                            points.Add(new DatePoint(j, i, 0));
                        }
                    }
                    if (i == 0 || j == 0 || i == ImagePKK.Height - 1 || j == ImagePKK.Width - 1)
                    {
                        points.Add(new DatePoint(j, i, 0));
                    }             
                }
            }

            #region поиск значений цыетов
            //List<DatePoint> pointsColor = new List<DatePoint>();
            //List<string> pointsstr = new List<string>();
            //for (int i = 0; i < ImagePKK.Height; i++)
            //{
            //    for (int j = 0; j < ImagePKK.Width; j++)
            //    {
            //        if (ImagePKK.Data[i, j, 2] != 0)
            //        {
            //            pointsstr.Add(ImagePKK.Data[i, j, 2].ToString());
            //        }
            //        //for (int k = 0; k < 3; k++)
            //        //{
            //        //    if (ImagePKK.Data[i, j, k] != 0)
            //        //    {
            //        //        pointsColor.Add(new DatePoint(j, i, k, ImagePKK.Data[i, j, k]));
            //        //    }
            //        //}
            //    }
            //}
            //var a = pointsstr.Distinct().ToList();
            //List<int> list = new List<int>();
            //foreach (var item in a)
            //{
            //    list.Add(int.Parse(item));
            //}
            //list.Sort();

            //for (int i = 0; i < ImagePKK.Height; i++)
            //{
            //    for (int j = 0; j < ImagePKK.Width; j++)
            //    {
            //        if (ImagePKK.Data[i, j, 1] != 0)
            //        {
            //            pointsstr.Add(ImagePKK.Data[i, j, 1].ToString());
            //        }
            //        //for (int k = 0; k < 3; k++)
            //        //{
            //        //    if (ImagePKK.Data[i, j, k] != 0)
            //        //    {
            //        //        pointsColor.Add(new DatePoint(j, i, k, ImagePKK.Data[i, j, k]));
            //        //    }
            //        //}
            //    }
            //}
            //a = pointsstr.Distinct().ToList();
            //list = new List<int>();
            //foreach (var item in a)
            //{
            //    list.Add(int.Parse(item));
            //}
            //list.Sort();
            #endregion

            foreach (var item in points)
            {
                ImagePlace.Data[item._y, item._x, 0] = 255;
            }

            return ImagePlace;
        }

        #endregion

        #region Работа с СЗЗ
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
        #endregion

        #region Переводы форматов
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
        #endregion

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

using OmoEReceLib.ERObjects;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace OmoOmotegaki.Controls
{
    public sealed class ShishikiImageSourceConverter : IValueConverter
    {
        public static int ImageWidth { get { return 200; } }
        public static int ImageHeight { get { return 50; } }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ER歯式 shishiki;

            if (value == null)
            {
                shishiki = null;
            }
            else
            {
                shishiki = value as ER歯式;
                if (shishiki == null)
                {
                    string erCode = value as string;
                    if (string.IsNullOrEmpty(erCode))
                    {
                        shishiki = null;
                    }
                    else
                    {
                        shishiki = ER歯式.FromERCode(erCode);
                    }
                }
            }

            using (var img = new Bitmap(ImageWidth, ImageHeight))
            {
                DrawShishiki(img, shishiki);

                using (var memory = new System.IO.MemoryStream())
                {
                    img.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = memory;
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();

                    return bmp;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static void DrawShishiki(Bitmap img, ER歯式 shishiki)
        {
            using (var g = Graphics.FromImage(img))
            using (var pen = new Pen(Brushes.DarkGoldenrod, 1f))
            {
                if (shishiki == null)
                {
                    g.DrawLine(pen, 0, img.Height, img.Width, 0);
                }
                else
                {
                    using (var font = new Font(System.Drawing.SystemFonts.DefaultFont.FontFamily, 10f))
                    {
                        OmoSeitokuEreceipt.Drawing.ERObjectDrawer.DrawSisikiFixed(
                            g, pen, shishiki, new string[0],
                            new RectangleF(0, 0, img.Width, img.Height),
                            font, true
                            );
                    }
                }
            }
        }
    }
}

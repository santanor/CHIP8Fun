using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;

namespace CHIP8Fun.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Emulator emulator;

        public MainWindow()
        {
            InitializeComponent();

            var thread = new Thread(() =>
            {
                emulator = new Emulator();
                emulator.Run();
            });

            thread.Start();

            while (emulator == null)
            {
                ;
            }

            KeyDown += emulator.OnKeyPressed;
            emulator.OnNewFrame += OnImageChanged;
        }

        private void OnImageChanged(Bitmap bmpSource)
        {
            bmp.Dispatcher.Invoke(() =>
            {
                var ms = new MemoryStream();
                bmpSource.Save(ms, ImageFormat.Bmp);
                var image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                bmp.Source = image;
            });
        }

        private void ResizeImage(ref Bitmap image, int canvasWidth, int canvasHeight)
        {
            var resultBmp = new Bitmap(canvasWidth, canvasHeight); // changed parm names
            var graphic = Graphics.FromImage(resultBmp);

            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
            // graphic.SmoothingMode = SmoothingMode.None;
            // graphic.PixelOffsetMode = PixelOffsetMode.Half;
            // graphic.CompositingQuality = CompositingQuality.AssumeLinear;

            /* ------------------ new code --------------- */

            // Figure out the ratio
            var ratioX = canvasWidth / (double)32;
            var ratioY = canvasHeight / (double)64;
            // use whichever multiplier is smaller
            var ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            var newHeight = Convert.ToInt32(32 * ratio);
            var newWidth = Convert.ToInt32(64 * ratio);

            // Now calculate the X,Y position of the upper-left corner
            // (one of these will always be zero)
            var posX = Convert.ToInt32((canvasWidth - 32 * ratio) / 2);
            var posY = Convert.ToInt32((canvasHeight - 64 * ratio) / 2);

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);

            //return resultBmp;
        }
    }
}

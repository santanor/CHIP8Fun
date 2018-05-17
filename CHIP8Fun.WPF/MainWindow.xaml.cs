using System.Drawing;
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
            var ms = new MemoryStream();
            bmpSource.Save(ms, ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze();

            bmp.Dispatcher.Invoke(() => {bmp.Source = image;});
        }
    }
}

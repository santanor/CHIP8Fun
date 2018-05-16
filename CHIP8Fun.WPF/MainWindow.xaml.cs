using System.Drawing;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace CHIP8Fun.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Emulator emulator;
        private ImageSource imgSource;

        public MainWindow()
        {
            InitializeComponent();

            var thread = new Thread(() =>
            {
                emulator = new Emulator();
                emulator.Run(out imgSource);
            });

            thread.Start();

            while (imgSource == null);

            KeyDown += emulator.OnKeyPressed;
            imgSource.Freeze();
            bmp.Source = imgSource;
        }
    }
}

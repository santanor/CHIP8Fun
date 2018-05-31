using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CHIP8Fun.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Emulator emulator;
        private TextBox[] registersValues;

        public MainWindow()
        {
            InitializeComponent();

            var thread = new Thread(() =>
            {
                emulator = new Emulator();
                emulator.Run();
            });

            thread.Start();

            //Wait for the emulator to be available
            while (emulator?.IsRunning != true)
            {}

            InitializeRegistersGrid();

            KeyDown += Emulator.OnKeyPressed;
            KeyUp += Emulator.OnKeyReleased;
            emulator.OnNewFrame += OnImageChanged;
        }

        private void InitializeRegistersGrid()
        {
            registersValues = new TextBox[Emulator.Chip8.V.Length];
            for (var i = 0; i < Emulator.Chip8.V.Length; i++)
            {
                var rowDef = new RowDefinition();
                RegistersGrid.RowDefinitions.Add(rowDef);

                var regName = new TextBlock {Text = $"V{i}"};
                Grid.SetColumn(regName, 0);
                Grid.SetRow(regName, i);

                var regValue = new TextBox
                {
                    IsEnabled = false,
                    Text = $"V{i}"
                };
                registersValues[i] = regValue;
                Grid.SetColumn(regValue, 1);
                Grid.SetRow(regValue, i);

                RegistersGrid.Children.Add(regName);
                RegistersGrid.Children.Add(regValue);
            }
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
    }
}

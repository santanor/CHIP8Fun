using System;
using System.Diagnostics;
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
        private TextBox[] memoryValues;

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
            InitializeMemoryGrid();

            KeyDown += Emulator.OnKeyPressed;
            KeyUp += Emulator.OnKeyReleased;
            emulator.OnNewFrame += OnImageChanged;
        }

        /// <summary>
        /// Initializes the grid that holds the data for the memory
        /// </summary>
        private void InitializeMemoryGrid()
        {
            memoryValues = new TextBox[Emulator.Chip8.Memory.Length];

            for (var i = 0; i < memoryValues.Length; i++)
            {
                //var memPosName = new Label(){Content = i.ToString()};

                var memValue = new TextBox()
                {
                    IsEnabled = false,
                    Text = i.ToString()
                };

                memoryValues[i] = memValue;
            }
            MemoryGrid.ItemsSource = memoryValues;
        }

        /// <summary>
        /// Initializes the grid that holds the data for the registers
        /// </summary>
        private void InitializeRegistersGrid()
        {
            registersValues = new TextBox[Emulator.Chip8.V.Length];
            for (var i = 0; i < Emulator.Chip8.V.Length; i++)
            {
                RegistersGrid.RowDefinitions.Add(new RowDefinition());

                var regName = new Label(){Content = $"V{i}"};
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

        /// <summary>
        /// Refreshes the image on the WPF application
        /// </summary>
        /// <param name="bmpSource"></param>
        private void OnImageChanged(Bitmap bmpSource)
        {
            try
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
            catch (Exception e)
            {
                Debug.WriteLine("Thank you for playing Wing Commander");
            }

        }
    }
}

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
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
        private TextBox[] keyValues;
        private TextBox[] memoryValues;
        private TextBox[] registersValues;
        private TextBox[] stackValues;

        public MainWindow()
        {
            InitializeComponent();

            //Starts the emulator
            var thread = new Thread(() =>
            {
                emulator = new Emulator();
                emulator.Run();
            });

            thread.Start();

            //Wait for the emulator to be available
            while (emulator?.IsRunning != true)
            {
                Thread.Sleep(5);
            }

            //Build the UI to display the debug data
            InitializeDebugDataGrid(ref RegistersGrid, ref registersValues, ref emulator.Chip8.V);
            InitializeDebugDataGrid(ref MemoryGrid, ref memoryValues, ref emulator.Chip8.Memory);
            InitializeDebugDataGrid(ref KeysGrid, ref keyValues, ref emulator.Chip8.Keys);
            InitializeDebugDataGrid(ref StackGrid, ref stackValues, ref emulator.Chip8.Stack);

            //Hook the events
            KeyDown += emulator.OnKeyPressed;
            KeyUp += emulator.OnKeyReleased;
            emulator.OnNewFrame += OnImageChanged;
            emulator.OnDebugTick += OnRefreshDebugGrids;
        }

        /// <summary>
        /// For each value, creates a grid with two columns, the index and the value. It then
        /// adds each grid to the listView passed as a parameter
        /// </summary>
        /// <param name="component"></param>
        /// <param name="gridValues"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        private void InitializeDebugDataGrid<T>(ref ListView component, ref TextBox[] gridValues, ref T[] data)
        {
            gridValues = new TextBox[data.Length];

            for (var i = 0; i < gridValues.Length; i++)
            {
                var dataName = new Label
                {
                    Content = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var dataValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                gridValues[i] = dataValue;

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50)
                });

                Grid.SetColumn(dataName, 0);
                Grid.SetColumn(dataValue, 1);

                grid.Children.Add(dataName);
                grid.Children.Add(dataValue);

                component.Items.Add(grid);
            }
        }

        /// <summary>
        /// Refreshes the image on the WPF application
        /// We have to use the Dispatcher because we're accesing the UI thread from a worker thread
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

        /// <summary>
        /// Refreshes all the grids that hold the emulator data
        /// We have to use the Dispatcher because we're accesing the UI thread from a worker thread
        /// </summary>
        private void OnRefreshDebugGrids()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    RefreshDebugDataGrid(ref memoryValues, ref emulator.Chip8.Memory);
                    RefreshDebugDataGrid(ref keyValues, ref emulator.Chip8.Keys);
                    RefreshDebugDataGrid(ref stackValues, ref emulator.Chip8.Stack);
                    RefreshDebugDataGrid(ref registersValues, ref emulator.Chip8.V);

                    PCValue.Text = emulator.Chip8.Pc.ToString();
                    SPValue.Text = emulator.Chip8.Sp.ToString();
                    IValue.Text = emulator.Chip8.I.ToString();
                    DelayTimerValue.Text = emulator.Chip8.DelayTimer.ToString();
                    SoundtimerValue.Text = emulator.Chip8.SoundTimer.ToString();
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Thank you for playing Wing Commander");
            }
        }


        /// <summary>
        /// Only updates the UI if the value has changed
        /// </summary>
        /// <param name="gridValues"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        private void RefreshDebugDataGrid<T>(ref TextBox[] gridValues, ref T[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                if (gridValues[i].Text != data[i].ToString())
                {
                    gridValues[i].Text = data[i].ToString();
                }
            }
        }
    }
}

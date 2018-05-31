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

            var thread = new Thread(() =>
            {
                emulator = new Emulator();
                emulator.Run();
            });

            thread.Start();

            //Wait for the emulator to be available
            while (emulator?.IsRunning != true)
            {
            }

            InitializeRegistersGrid();
            InitializeMemoryGrid();
            InitializeStackGrid();
            InitializeKeysGrid();

            KeyDown += emulator.OnKeyPressed;
            KeyUp += emulator.OnKeyReleased;
            emulator.OnNewFrame += OnImageChanged;
            emulator.OnDebugTick += OnRefreshDebugGrids;
        }

        /// <summary>
        /// Initializes the grid that holds the data for the keys
        /// </summary>
        private void InitializeKeysGrid()
        {
            keyValues = new TextBox[emulator.Chip8.Keys.Length];

            for (var i = 0; i < keyValues.Length; i++)
            {
                var keyName = new Label
                {
                    Content = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var keyValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                keyValues[i] = keyValue;

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50)
                });

                Grid.SetColumn(keyName, 0);
                Grid.SetColumn(keyValue, 1);

                grid.Children.Add(keyName);
                grid.Children.Add(keyValue);

                KeysGrid.Items.Add(grid);
            }
        }

        /// <summary>
        /// Initializes the grid that holds the data for the stack
        /// </summary>
        private void InitializeStackGrid()
        {
            stackValues = new TextBox[emulator.Chip8.Stack.Length];

            for (var i = 0; i < stackValues.Length; i++)
            {
                var stackName = new Label
                {
                    Content = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var stackValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                stackValues[i] = stackValue;

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50)
                });

                Grid.SetColumn(stackName, 0);
                Grid.SetColumn(stackValue, 1);

                grid.Children.Add(stackName);
                grid.Children.Add(stackValue);

                StackGrid.Items.Add(grid);
            }
        }

        /// <summary>
        /// Initializes the grid that holds the data for the memory
        /// </summary>
        private void InitializeMemoryGrid()
        {
            memoryValues = new TextBox[emulator.Chip8.Memory.Length];

            for (var i = 0; i < memoryValues.Length; i++)
            {
                var memName = new Label
                {
                    Content = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var memValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                memoryValues[i] = memValue;

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50)
                });

                Grid.SetColumn(memName, 0);
                Grid.SetColumn(memValue, 1);

                grid.Children.Add(memName);
                grid.Children.Add(memValue);

                MemoryGrid.Items.Add(grid);
            }
        }

        /// <summary>
        /// Initializes the grid that holds the data for the registers
        /// </summary>
        private void InitializeRegistersGrid()
        {
            registersValues = new TextBox[emulator.Chip8.V.Length];

            for (var i = 0; i < registersValues.Length; i++)
            {
                var regName = new Label
                {
                    Content = $"V{i}",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var regValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                registersValues[i] = regValue;

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50)
                });

                Grid.SetColumn(regName, 0);
                Grid.SetColumn(regValue, 1);

                grid.Children.Add(regName);
                grid.Children.Add(regValue);

                RegistersGrid.Items.Add(grid);
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

        /// <summary>
        /// Refreshes all the grids that hold the emulator data
        /// </summary>
        private void OnRefreshDebugGrids()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    for (var i = 0; i < memoryValues.Length; i++)
                    {
                        memoryValues[i].Text = emulator.Chip8.Memory[i].ToString("X");
                    }

                    for (var i = 0; i < keyValues.Length; i++)
                    {
                        keyValues[i].Text = emulator.Chip8.Keys[i].ToString("X");
                    }

                    for (var i = 0; i < stackValues.Length; i++)
                    {
                        stackValues[i].Text = emulator.Chip8.Stack[i].ToString();
                    }

                    for (var i = 0; i < registersValues.Length; i++)
                    {
                        registersValues[i].Text = emulator.Chip8.V[i].ToString();
                    }

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
    }
}

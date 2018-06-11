using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using CHIP8Fun.Disassembler;
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;

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

        /// <summary>
        /// Used tp hold a reference to the previous instruction selected in
        /// the dissasembled window.
        /// </summary>
        private Block selectedInstruction;

        private TextBox[] registersValues;
        private TextBox[] stackValues;
        private IDictionary<int, Paragraph> dissasembledBlocks;

        private Thread emulatorThread;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeEmulator(string fileName, bool debug)
        {
            emulatorThread = new Thread(() =>
            {
                emulator = new Emulator {Debug = debug};
                emulator.Run(fileName);
            });
            emulatorThread.Start();

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
            emulator.OnUiTick += OnRefreshDebugGrids;

            //Initialize the dissasembler
            var disassembler = new Disassembler.Disassembler(emulator.ProgramCode);
            FormatDissasembledCode(disassembler.Dissasemble());
        }

        private void FormatDissasembledCode(DissasembledModel dissasemble)
        {
            dissasembledBlocks = new Dictionary<int, Paragraph>();
            foreach (var dlm in dissasemble.LineModels)
            {
                //Only one Span per instruction will be created
                var instructionParagraph = new Paragraph();

                //Inside this Run we'll store the helper text like the deescription and the instruction
                var descriptionText = new Run {Foreground = Brushes.DarkGray};
                var sb = new StringBuilder();
                sb.AppendLine(dlm.Description);
                sb.Append("\t").AppendLine(dlm.AssemblyCode);
                descriptionText.Text = sb.ToString();

                //This run will have the opcode
                var opcodeText = new Run
                {
                    FontWeight = FontWeights.Bold,
                    Text = $"\t{dlm.Opcode}\n"
                };

                instructionParagraph.Inlines.Add(descriptionText);
                instructionParagraph.Inlines.Add(opcodeText);

                DissasembledCode.Blocks.Add(instructionParagraph);
                dissasembledBlocks.Add(dlm.PositionInMemory, instructionParagraph);
            }
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
                var dataName = new TextBlock()
                {
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var dataValue = new TextBox
                {
                    IsEnabled = false,
                    Text = i.ToString("X"),
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
                Environment.Exit(0);
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
                    OpcodeValue.Text = emulator.Chip8.Opcode.ToString("X");
                    DelayTimerValue.Text = emulator.Chip8.DelayTimer.ToString();
                    SoundtimerValue.Text = emulator.Chip8.SoundTimer.ToString();

                    SelectAndScrollValuesIntoView();
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Thank you for playing Wing Commander");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Focuses on the values defined by SP and PC. It also jumps to that position
        /// </summary>
        private void SelectAndScrollValuesIntoView()
        {
            MemoryGrid.SelectedIndex = emulator.Chip8.Pc;
            MemoryGrid.ScrollIntoView(MemoryGrid.SelectedItem);

            StackGrid.SelectedIndex = emulator.Chip8.Sp;
            StackGrid.ScrollIntoView(StackGrid.SelectedIndex);

            //For the dissasembled code
            if (emulator.Debugger.Paused || emulator.Debugger.StepOver)
            {
                if (selectedInstruction != null)
                {
                    selectedInstruction.Background = Brushes.Transparent;
                }

                var pcValue = emulator.Chip8.Pc % 2 == 0 ? emulator.Chip8.Pc - 2 : emulator.Chip8.Pc - 1;
                if (dissasembledBlocks.TryGetValue(pcValue, out var instruction))
                {
                    //We do Pc-2 because on Pc will be the next instruction, not the one stopped at
                    selectedInstruction = instruction;
                    selectedInstruction.Background = Brushes.Yellow;
                    selectedInstruction.BringIntoView();
                }
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

        private void MenuRunRom(object sender, RoutedEventArgs e)
        {
            ShowDialogAndRun(true);
        }

        private void MenuDebugRom(object sender, RoutedEventArgs e)
        {
            ShowDialogAndRun(true);
        }

        private void ShowDialogAndRun(bool debug)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".ch8";

            var result = fileDialog.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                var filename = fileDialog.FileName;
                InitializeEmulator(filename, debug);
            }
        }

        private void MenuSetClockSpeed(object sender, RoutedEventArgs e)
        {
            var speed = ((MenuItem)sender).Tag.ToString();
            emulator.Chip8.ClockSpeed = int.Parse(speed);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Input;

namespace CHIP8Fun
{
    public class Emulator
    {
        public delegate void UIEvent();
        public delegate void NewFrame(Bitmap img);
        public delegate void KeyPressedEvent(KeyEventArgs keyEventArgs);

        private Bitmap backingImage;
        public CHIP8System Chip8;
        public bool IsRunning;
        public const bool Debug = true;
        private Debugger debugger;
        private double timerCounter;
        public byte[] ProgramCode;

        public UIEvent OnUiTick;
        public NewFrame OnNewFrame;
        public KeyPressedEvent OnUIKeyDown;
        public KeyPressedEvent OnUIKeyUp;

        /// <summary>
        /// Map of the keys used by the emulator
        /// </summary>
        private readonly IDictionary<Key, int> keyMap = new Dictionary<Key, int>
        {
            {Key.D1, 0},
            {Key.D2, 1},
            {Key.D3, 2},
            {Key.D4, 3},
            {Key.Q, 4},
            {Key.W, 5},
            {Key.E, 6},
            {Key.R, 7},
            {Key.A, 8},
            {Key.S, 9},
            {Key.D, 10},
            {Key.F, 11},
            {Key.Z, 12},
            {Key.X, 13},
            {Key.C, 14},
            {Key.V, 15}
        };



        /// <summary>
        /// Emulator Loop
        /// </summary>
        public void Run()
        {
            // Initialize the Chip8 system and load the game into the memory
            Chip8 = new CHIP8System(backingImage);
            ProgramCode = Chip8.LoadProgram("Tetris.ch8");
            IsRunning = true;

            if (Debug)
            {
                debugger = new Debugger(this);
                debugger.Run();
            }
            else
            {
                RunNoDebugger();
            }
        }



        /// <summary>
        /// Runs the program without the debugger attached, it'll still print debug instructions to the UI
        /// </summary>
        private void RunNoDebugger()
        {
            while (true)
            {
                // Emulate one cycle
                Chip8.EmulateCycle();
                TryPresentFrame();
                TryUpdateUiData();
            }
        }

        /// <summary>
        /// Updates the UI data if the last call happened more than 20 miliseconds ago
        /// </summary>
        public void TryUpdateUiData()
        {
            //Notifies the debugger to do its thing
            var currentTime = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
            var milisecondsSinceLastUpdate = currentTime - timerCounter;

            //more than 1/60th of a second passed
            if (milisecondsSinceLastUpdate > 20)
            {
                timerCounter = currentTime;
                OnUiTick?.Invoke();
            }
        }

        /// <summary>
        /// If the draw flag is set, update the screen
        /// </summary>
        public void TryPresentFrame()
        {
            if (Chip8.V[15] == 1)
            {
                var newFrame = DrawGraphics();
                OnNewFrame?.Invoke(newFrame);
                Chip8.V[15] = 0;
            }
        }

        /// <summary>
        /// Presents the backBuffer to the displaying image in the frontend
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawGraphics()
        {
            var bmp = new Bitmap(64, 32);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 32; j++)
                {
                    bmp.SetPixel(i, j, Chip8.Gfx[i, j] ? Color.White : Color.Black);
                }
            }

            return bmp;
        }

        /// <summary>
        /// If the key pressed is in the keymap for the emulator, assigns the value of the key to the cpu's register
        /// </summary>
        public void OnKeyPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyMap.ContainsKey(keyEventArgs.Key))
            {
                Chip8.Keys[keyMap[keyEventArgs.Key]] = (byte)keyEventArgs.Key;
            }
            OnUIKeyDown?.Invoke(keyEventArgs);
        }

        /// <summary>
        /// If the key released is in the keymap for the emulator, clears the value on the cpu's register
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keyEventArgs"></param>
        public void OnKeyReleased(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyMap.ContainsKey(keyEventArgs.Key))
            {
                Chip8.Keys[keyMap[keyEventArgs.Key]] = 0;
            }
            OnUIKeyUp?.Invoke(keyEventArgs);
        }
    }
}

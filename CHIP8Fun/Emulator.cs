﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace CHIP8Fun
{
    public class Emulator
    {
        public delegate void NewFrame(Bitmap img);

        public delegate void DebugEvent();

        public NewFrame OnNewFrame;
        public DebugEvent OnDebugTick;
        public CHIP8System Chip8;
        private Bitmap backingImage;
        public bool IsRunning;
        private double timerCounter;

        /// <summary>
        /// Emulator Loop
        /// </summary>
        public void Run()
        {// Initialize the Chip8 system and load the game into the memory
            Chip8 = new CHIP8System(backingImage);
            Chip8.LoadProgram("Tetris.ch8");
            IsRunning = true;

            // Emulation loop
            while (true)
            {
                // Emulate one cycle
                Chip8.EmulateCycle();

                // If the draw flag is set, update the screen
                if (Chip8.V[15] == 1)
                {
                    var newFrame = DrawGraphics();
                    OnNewFrame?.Invoke(newFrame);
                    Chip8.V[15] = 0;
                }

                //Notifies the debugger to do its thing
                var currentTime = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
                var milisecondsSinceLastUpdate = currentTime - timerCounter;

                //more than 1/60th of a second passed
                if (milisecondsSinceLastUpdate > 50)
                {
                    timerCounter = currentTime;
                    OnDebugTick?.Invoke();
                }
            }
        }

        private Bitmap DrawGraphics()
        {
            var bmp = new Bitmap(64,32);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 32; j++)
                {
                    bmp.SetPixel(i,j, Chip8.Gfx[i,j] ? Color.White : Color.Black);
                }
            }
            return bmp;
        }

        public void OnKeyPressed(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.D1:
                    Chip8.Keys[0] = Convert.ToByte(keyEventArgs.Key);
                    break;
                case Key.D2:
                    Chip8.Keys[1] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D3:
                    Chip8.Keys[2] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D4:
                    Chip8.Keys[3] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.Q:
                    Chip8.Keys[4] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.W:
                    Chip8.Keys[5] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.E:
                    Chip8.Keys[6] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.R:
                    Chip8.Keys[7] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.A:
                    Chip8.Keys[8] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.S:
                    Chip8.Keys[9] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D:
                    Chip8.Keys[10] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.F:
                    Chip8.Keys[11] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.Z:
                    Chip8.Keys[12] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.X:
                    Chip8.Keys[13] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.C:
                    Chip8.Keys[14] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.V:
                    Chip8.Keys[15] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
            }
        }

        public void OnKeyReleased(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.D1:
                    Chip8.Keys[0] = 0;
                    break;
                case Key.D2:
                    Chip8.Keys[1] = 0;
                    ;
                    break;
                case Key.D3:
                    Chip8.Keys[2] = 0;
                    ;
                    break;
                case Key.D4:
                    Chip8.Keys[3] = 0;
                    ;
                    break;
                case Key.Q:
                    Chip8.Keys[4] = 0;
                    ;
                    break;
                case Key.W:
                    Chip8.Keys[5] = 0;
                    ;
                    break;
                case Key.E:
                    Chip8.Keys[6] = 0;
                    ;
                    break;
                case Key.R:
                    Chip8.Keys[7] = 0;
                    ;
                    break;
                case Key.A:
                    Chip8.Keys[8] = 0;
                    ;
                    break;
                case Key.S:
                    Chip8.Keys[9] = 0;
                    ;
                    break;
                case Key.D:
                    Chip8.Keys[10] = 0;
                    ;
                    break;
                case Key.F:
                    Chip8.Keys[11] = 0;
                    ;
                    break;
                case Key.Z:
                    Chip8.Keys[12] = 0;
                    ;
                    break;
                case Key.X:
                    Chip8.Keys[13] = 0;
                    ;
                    break;
                case Key.C:
                    Chip8.Keys[14] = 0;
                    ;
                    break;
                case Key.V:
                    Chip8.Keys[15] = 0;
                    ;
                    break;
            }

        }
    }
}

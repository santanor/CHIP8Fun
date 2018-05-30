using System;
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

        public NewFrame OnNewFrame;
        private static CHIP8System chip8;
        private Bitmap backingImage;

        /// <summary>
        /// Emulator Loop
        /// </summary>
        public void Run()
        {// Initialize the Chip8 system and load the game into the memory
            chip8 = new CHIP8System(backingImage);
            chip8.LoadProgram("Tetris.ch8");

            // Emulation loop
            while (true)
            {
                // Emulate one cycle
                chip8.EmulateCycle();

                // If the draw flag is set, update the screen
                if (chip8.V[15] == 1)
                {
                    var newFrame = DrawGraphics();
                    OnNewFrame?.Invoke(newFrame);
                    chip8.V[15] = 0;
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
                    bmp.SetPixel(i,j, chip8.Gfx[i,j] ? Color.Black : Color.White);
                }
            }
            return bmp;
        }

        public static void OnKeyPressed(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.D1:
                    chip8.Keys[0] = Convert.ToByte(keyEventArgs.Key);
                    break;
                case Key.D2:
                    chip8.Keys[1] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D3:
                    chip8.Keys[2] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D4:
                    chip8.Keys[3] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.Q:
                    chip8.Keys[4] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.W:
                    chip8.Keys[5] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.E:
                    chip8.Keys[6] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.R:
                    chip8.Keys[7] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.A:
                    chip8.Keys[8] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.S:
                    chip8.Keys[9] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.D:
                    chip8.Keys[10] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.F:
                    chip8.Keys[11] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.Z:
                    chip8.Keys[12] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.X:
                    chip8.Keys[13] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.C:
                    chip8.Keys[14] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
                case Key.V:
                    chip8.Keys[15] = Convert.ToByte(keyEventArgs.Key);
                    ;
                    break;
            }
        }

        public static void OnKeyReleased(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.D1:
                    chip8.Keys[0] = 0;
                    break;
                case Key.D2:
                    chip8.Keys[1] = 0;
                    ;
                    break;
                case Key.D3:
                    chip8.Keys[2] = 0;
                    ;
                    break;
                case Key.D4:
                    chip8.Keys[3] = 0;
                    ;
                    break;
                case Key.Q:
                    chip8.Keys[4] = 0;
                    ;
                    break;
                case Key.W:
                    chip8.Keys[5] = 0;
                    ;
                    break;
                case Key.E:
                    chip8.Keys[6] = 0;
                    ;
                    break;
                case Key.R:
                    chip8.Keys[7] = 0;
                    ;
                    break;
                case Key.A:
                    chip8.Keys[8] = 0;
                    ;
                    break;
                case Key.S:
                    chip8.Keys[9] = 0;
                    ;
                    break;
                case Key.D:
                    chip8.Keys[10] = 0;
                    ;
                    break;
                case Key.F:
                    chip8.Keys[11] = 0;
                    ;
                    break;
                case Key.Z:
                    chip8.Keys[12] = 0;
                    ;
                    break;
                case Key.X:
                    chip8.Keys[13] = 0;
                    ;
                    break;
                case Key.C:
                    chip8.Keys[14] = 0;
                    ;
                    break;
                case Key.V:
                    chip8.Keys[15] = 0;
                    ;
                    break;
            }

        }
    }
}

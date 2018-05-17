// ReSharper disable InconsistentNaming

using System;
using System.Drawing;

namespace CHIP8Fun
{
    public class Opcodes
    {
        private readonly CHIP8System s;

        public Opcodes(CHIP8System s)
        {
            this.s = s;
        }

        /// <summary>
        /// MEM
        /// Sets I to the address NNN.
        /// </summary>
        /// <param name="code"></param>
        public void ANNN(short code)
        {
            Console.WriteLine($"Executing: {code:X}");
            s.I = (short)(code & 0x0FFF);
            s.Pc += 2;
        }

        /// <summary>
        /// KeyOp
        /// A key press is awaited, and then stored in VX.
        /// (Blocking Operation. All instruction halted until next key event)
        /// </summary>
        /// <param name="code"></param>
        public void FX0A(short code)
        {
            Console.WriteLine($"Executing: {code:X}");
            var X = (code & 0x0F00) >> 8;
            //Blocking operation. Waits for a key to be pressed
            var key = s.AnyKeyPressed();
            if (key != 0)
            {
                s.V[X] = s.AnyKeyPressed();
                s.Pc += 2;
            }
        }

        /// <summary>
        /// Display
        /// Clears the screen.
        /// </summary>
        public void _00E0()
        {
            for (var i = 0; i < s.Gfx.GetLength(0); i++)
            {
                for (var j = 0; j < s.Gfx.GetLength(1); j++)
                {
                    s.Gfx[i,j] = 255;
                }
            }

            s.V[15] = 1;
            s.Pc += 2;
        }
    }
}

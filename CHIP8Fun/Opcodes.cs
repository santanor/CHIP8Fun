// ReSharper disable InconsistentNaming

using System;

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
            var key = Console.ReadKey();
            Console.Write("\b");
            var X = (code & 0x0F00) >> 8;
            s.V[X] = Convert.ToByte(key.KeyChar);
            s.Pc += 2;
        }
    }
}

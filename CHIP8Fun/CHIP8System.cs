using System;
using System.IO;

namespace CHIP8Fun
{
    // ReSharper disable once InconsistentNaming
    public class CHIP8System
    {
        /// <summary>
        /// Initialize system
        /// Before running the first emulation cycle, you will need to prepare your system state.
        /// Start clearing the memory and resetting the registers to zero.
        /// While the Chip 8 doesn’t really have a BIOS or firmware, it does have a basic fontset stored in the memory.
        /// The system expects the application to be loaded at memory location 0x200.
        /// This means that your program counter should also be set to this location.
        /// </summary>
        public CHIP8System()
        {
            Memory = new byte[4096];
            V = new byte[16];
            Gfx = new byte[64, 32];
            Stack = new short[16];
            Keys = new byte[16];

            Pc = 0x200; // Program counter starts at 0x200
            opcode = 0; // Reset current opcode
            I = 0; // Reset index register
            Sp = 0; // Reset stack pointer

            // Clear display
            // Clear stack
            // Clear registers V0-VF
            // Clear memory

            // Load fontset
            for (var i = 0; i < 80; ++i)
            {
                Memory[i] = chip8Fontset[i];
            }

            cpu = new Opcodes(this);


            // Reset timers
        }

        /// <summary>
        /// Fetch Opcode
        /// Decode Opcode
        /// Execute Opcode
        /// Update timers
        /// </summary>
        public void EmulateCycle()
        {
            opcode = FetchOpcode();
            DecodeOpcode(opcode);
            // Execute Opcode
            // Update timers
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            if (delayTimer > 0)
            {
                delayTimer--;
            }

            if (soundTimer > 0)
            {
                if (soundTimer == 1)
                {
                    Console.WriteLine("BEEP!");
                }

                soundTimer--;
            }
        }

        /// <summary>
        /// fetch one opcode from the memory at the location specified by the program counter (pc).
        /// In our Chip 8 emulator, data is stored in an array in which each address contains one byte.
        /// As one opcode is 2 bytes long,  we will need to fetch two successive bytes and merge them to get the opcode.
        /// </summary>
        /// <returns></returns>
        private short FetchOpcode()
        {
            return (short)((Memory[Pc] << 8) | Memory[Pc + 1]);
        }


        /// <summary>
        /// Here's where most of the magic happens, it routes the execution of the emulator decoding the opcode
        /// and executing what it has to do
        /// </summary>
        /// <param name="code"></param>
        private void DecodeOpcode(short code)
        {
            switch (code & 0xF000)
            {
                // Some opcodes //

                case 0xA000: // ANNN: Sets I to the address NNN
                    cpu.ANNN(code);
                    break;
                case 0xF000: // FX0A: Wait for key press, store key pressed in VX
                    cpu.FX0A(code);
                    break;

                //In some cases we can not rely solely on the first four bits to see what the opcode means.
                //For example, 0x00E0 and 0x00EE both start with 0x0.
                //In this case we add an additional switch and compare the last four bits:
                case 0x0000:
                {
                    switch (opcode & 0x000F)
                    {
                        case 0x0000: // 0x00E0: Clears the screen
                            // Execute opcode
                            break;

                        case 0x000E: // 0x00EE: Returns from subroutine
                            // Execute opcode
                            break;

                        default:
                            Console.WriteLine($"Unknown opcode [0x0000]: {opcode:X}");
                            break;
                    }

                    break;
                }


                default:
                    Console.WriteLine($"Unknown opcode: {opcode:X}");
                    break;
            }
        }

        /// <summary>
        /// After you have initialized the emulator, load the program into the memory (in binary mode)
        /// and start filling the memory at location: 0x200 == 512.
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadProgram(string fileName)
        {
            try
            {
                var buffer = File.ReadAllBytes(fileName);
                for (var i = 0; i < buffer.Length; ++i)
                {
                    Memory[i + 512] = buffer[i];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region System specification

        /// <summary>
        /// The Chip 8 has 35 opcodes which are all two bytes long. To store the current opcode,
        /// we need a data type that allows us to store two bytes.
        /// </summary>
        private short opcode;

        /// <summary>
        /// The Chip 8 has 4K memory in total, which we can emulated as this
        /// </summary>
        public byte[] Memory;

        /// <summary>
        /// CPU registers: The Chip 8 has 15 8-bit general purpose registers named V0,V1 up to VE.
        /// The 16th register is used  for the ‘carry flag’.
        /// Eight bits is one byte so we can use an unsigned char for this purpose:
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public byte[] V;

        /// <summary>
        /// There is an Index register which can have a value from 0x000 to 0xFFF
        /// </summary>
        public short I;

        /// <summary>
        /// There is a program counter (pc) which can have a value from 0x000 to 0xFFF
        /// </summary>
        public short Pc;

        //The systems memory map:
        // 0x000-0x1FF - Chip 8 interpreter (contains font set in emu)
        // 0x050-0x0A0 - Used for the built in 4x5 pixel font set (0-F)
        // 0x200-0xFFF - Program ROM and work RAM

        /// <summary>
        /// The graphics of the Chip 8 are black and white and the screen has a total of 2048 pixels (64 x 32).
        /// This can easily be implemented using an array that hold the pixel state (1 or 0):
        /// </summary>
        public byte[,] Gfx;

        /// <summary>
        /// Interupts and hardware registers. The Chip 8 has none,
        /// but there are two timer registers that count at 60 Hz. When set above zero they will count down to zero.
        /// </summary>
        private byte delayTimer;

        /// <summary>
        /// The system’s buzzer sounds whenever the sound timer reaches zero.
        /// </summary>
        private byte soundTimer;

        /// <summary>
        /// The stack is used to remember the current location before a jump is performed.
        /// So anytime you perform a jump or call a subroutine store the program counter in the stack before proceeding.
        /// </summary>
        public short[] Stack;

        /// <summary>
        /// The system has 16 levels of stack and in order to remember which level of the stack is used,
        /// you need to implement a stack pointer (sp).
        /// </summary>
        public short Sp;

        /// <summary>
        /// Finally, the Chip 8 has a HEX based keypad (0x0-0xF),
        /// you can use an array to store the current state of the key.
        /// </summary>
        public  byte[] Keys;

        private readonly Opcodes cpu;

        #region fontset

        private readonly byte[] chip8Fontset =
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80 // F
        };

        #endregion

        #endregion
    }
}

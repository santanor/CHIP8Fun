﻿// ReSharper disable InconsistentNaming
using System;
using System.Diagnostics;
#pragma warning disable 1570

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
        /// Call
        /// Calls RCA 1802 program at address NNN. Not necessary for most ROMs.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _0NNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Display
        /// Clears the screen.
        /// </summary>
        public void _00E0()
        {
            Debug.WriteLine($"Executing: 00E0");
            for (var i = 0; i < s.Gfx.GetLength(0); i++)
            {
                for (var j = 0; j < s.Gfx.GetLength(1); j++)
                {
                    s.Gfx[i, j] = 255;
                }
            }

            s.V[15] = 1;
            s.Pc += 2;
        }

        /// <summary>
        /// Flow
        /// Returns from a subroutine
        /// </summary>
        /// <param name="code"></param>
        public void _00EE(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Flow
        /// goto NNN. Jumps to address NNN
        /// </summary>
        /// <param name="code"></param>
        public void _1NNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Flow
        /// Calls subroutine at NNN
        /// </summary>
        /// <param name="code"></param>
        public void _2NNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Cond
        /// Skips the next statement if VX equals NN. (Usually the next instruction is a jump to skip
        /// a code block)
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _3XNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Cond
        /// Skips the next istruction if VX doesn't equal NN
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _4XNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Cond
        /// Skips the next instruction if VX equals VY.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _5XY0(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Const
        /// Sets VX to NN
        /// </summary>
        /// <param name="code"></param>
        public void _6XNN(short code)
        {
            Debug.WriteLine($"Executing: {code:X}");
            var X = (code & 0x0F00) >> 8;
            var NN = code & 0x00FF;
            s.V[X] = (byte)NN;
            s.Pc += 2;
        }

        /// <summary>
        /// Const
        /// Adds NN to VX. (Carry flag is not changed)
        /// </summary>
        /// <param name="code"></param>
        public void _7XNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Assign
        /// Sets VX to the value of VY
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY0(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BitOp Vx=Vx|Vy
        /// Sets VX to VX or VY. (Bitwise OR operation)
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY1(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BitOp Vx=Vx&Vy
        /// Sets VX to VX and VY. (Bitwise AND operation)
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY2(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BitOp Vx=Vx^Vy
        /// Sets VX to VX xor VY.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY3(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Math Vx += Vy
        /// Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY4(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Math Op Vx -= Vy
        /// VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY5(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BitOp	Vx=Vy=Vy>>1
        /// Shifts VY right by one and stores the result to VX (VY remains unchanged).
        /// VF is set to the value of the least significant bit of VY before the shift.[2]
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY6(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Math	Vx=Vy-Vx
        /// Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY7(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BitOp
        /// Shifts VY left by one and copies the result to VX.
        /// VF is set to the value of the most significant bit of VY before the shift.
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XYE(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Cond	if(Vx!=Vy)
        /// Skips the next instruction if VX doesn't equal VY.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _9XY0(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// MEM
        /// Sets I to the address NNN.
        /// </summary>
        /// <param name="code"></param>
        public void ANNN(short code)
        {
            Debug.WriteLine($"Executing: {code:X}");
            s.I = (short)(code & 0x0FFF);
            s.Pc += 2;
        }

        /// <summary>
        /// Flow	PC=V0+NNN
        /// Jumps to the address NNN plus V0.
        /// </summary>
        /// <param name="code"></param>
        public void BNNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Rand	Vx=rand()&NN
        /// Sets VX to the result of a bitwise and operation on a random number (Typically: 0 to 255) and NN.
        /// </summary>
        /// <param name="code"></param>
        public void CXNN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Disp	draw(Vx,Vy,N)
        /// Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels.
        /// Each row of 8 pixels is read as bit-coded starting from memory location I; I value doesn’t
        /// change after the execution of this instruction. As described above,
        /// VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn,
        /// and to 0 if that doesn’t happen
        /// </summary>
        /// <param name="code"></param>
        public void DXYN(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// KeyOp	if(key()==Vx)
        /// Skips the next instruction if the key stored in VX is pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void EX9E(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// KeyOp	if(key()!=Vx)
        /// Skips the next instruction if the key stored in VX isn't pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void EXA1(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Timer	Vx = get_delay()
        /// Sets VX to the value of the delay timer.
        /// </summary>
        /// <param name="code"></param>
        public void FX07(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// KeyOp
        /// A key press is awaited, and then stored in VX.
        /// (Blocking Operation. All instruction halted until next key event)
        /// </summary>
        /// <param name="code"></param>
        public void FX0A(short code)
        {
            Debug.WriteLine($"Executing: {code:X}");
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
        /// Timer	delay_timer(Vx)
        /// Sets the delay timer to VX.
        /// </summary>
        /// <param name="code"></param>
        public void FX15(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// Sound	sound_timer(Vx)
        /// Sets the sound timer to VX.
        /// </summary>
        /// <param name="code"></param>
        public void FX18(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// MEM	I +=Vx
        /// Adds VX to I.[3]
        /// </summary>
        /// <param name="code"></param>
        public void FX1E(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// MEM	I=sprite_addr[Vx]
        /// Sets I to the location of the sprite for the character in VX.
        /// Characters 0-F (in hexadecimal) are represented by a 4x5 font.
        /// </summary>
        /// <param name="code"></param>
        public void FX29(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// BCD	set_BCD(Vx); *(I+0)=BCD(3); *(I+1)=BCD(2); *(I+2)=BCD(1);
        ///Stores the binary-coded decimal representation of VX,
        /// with the most significant of three digits at the address in I,
        /// the middle digit at I plus 1, and the least significant digit at I plus 2.
        /// (In other words, take the decimal representation of VX,
        /// place the hundreds digit in memory at location in I,
        /// the tens digit at location I+1, and the ones digit at location I+2.)
        /// </summary>
        /// <param name="code"></param>
        public void FX33(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// MEM	reg_dump(Vx,&I)
        /// Stores V0 to VX (including VX) in memory starting at address I. I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public void FX55(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }

        /// <summary>
        /// MEM	reg_load(Vx,&I)
        /// Fills V0 to VX (including VX) with values from memory starting at address I.
        /// I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public void FX65(short code)
        {
            throw new NotImplementedException($"code: {code:X} not implemented");
        }
    }
}

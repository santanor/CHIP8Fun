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
            throw new NotImplementedException($"code: {code:x} not implemented");
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
                    s.Gfx[i, j] = true;
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
            s.Sp--;
            s.Pc = s.Stack[s.Sp];
            s.Pc += 2;
        }

        /// <summary>
        /// Flow
        /// goto NNN. Jumps to address NNN
        /// </summary>
        /// <param name="code"></param>
        public void _1NNN(short code)
        {
            var address = code & 0xFFF;
            s.Pc = (short)address;
        }

        /// <summary>
        /// Flow
        /// Calls subroutine at NNN
        /// </summary>
        /// <param name="code"></param>
        public void _2NNN(short code)
        {
            if (s.Sp >= s.Stack.Length)
            {
                Debug.WriteLine($"Stack Overflow! opcode{code:x}");
                return;
            }

            var label = code & 0x0FFF; //get the label address from the opcode
            s.Stack[s.Sp] = s.Pc; //backup the current PC in the stack
            s.Sp++; //Increment the stack pointer to an empty position
            s.Pc = (short)label; //Move the PC to the label
        }

        /// <summary>
        /// Cond
        /// Skips the next statement if VX equals nn. (Usually the next instruction is a jump to skip
        /// a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _3XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;

            if (s.V[x] == nn)
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// Cond
        /// Skips the next istruction if VX doesn't equal nn
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _4XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;

            if (s.V[x] != nn)
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// Cond
        /// Skips the next instruction if VX equals VY.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _5XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            if (s.V[x] == s.V[y])
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// Const
        /// Sets VX to nn
        /// </summary>
        /// <param name="code"></param>
        public void _6XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = (byte)(code & 0x00FF);
            s.V[x] = nn;
            s.Pc += 2;
        }

        /// <summary>
        /// Const
        /// Adds nn to VX. (Carry flag is not changed)
        /// </summary>
        /// <param name="code"></param>
        public void _7XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;

            s.V[x] += (byte)nn;
            s.Pc += 2;
        }

        /// <summary>
        /// Assign
        /// Sets VX to the value of VY
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[x] = s.V[y];

            s.Pc += 2;
        }

        /// <summary>
        /// BitOp Vx=Vx|Vy
        /// Sets VX to VX or VY. (Bitwise OR operation)
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY1(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[x] |= s.V[y];

            s.Pc += 2;
        }

        /// <summary>
        /// BitOp Vx=Vx&Vy
        /// Sets VX to VX and VY. (Bitwise AND operation).
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void _8XY2(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[x] &= s.V[y];

            s.Pc += 2;
        }

        /// <summary>
        /// BitOp Vx=Vx^Vy
        /// Sets VX to VX xor VY.
        /// </summary>
        /// <param name="code"></param>
        public void _8XY3(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[x] ^= s.V[y];

            s.Pc += 2;
        }

        /// <summary>
        /// Math Vx += Vy
        /// Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public void _8XY4(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[s.VF] = s.V[x] + s.V[y] > 0xFF ? (byte)1 : (byte)0;

            s.V[x] += s.V[y];

            s.Pc += 2;
        }

        /// <summary>
        /// Math Op Vx -= Vy
        /// VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public void _8XY5(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[s.VF] = s.V[x] < s.V[y] ? (byte)0 : (byte)1;

            s.V[x] = (byte)(s.V[x] - s.V[y]);

            s.Pc += 2;
        }

        /// <summary>
        /// BitOp	Vx=Vy=Vy>>1
        /// Shifts VY right by one and stores the result to VX (VY remains unchanged).
        /// VF is set to the value of the least significant bit of VY before the shift.
        /// </summary>
        /// <param name="code"></param>
        public void _8XY6(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[x] = (byte)(s.V[x] >> 1);
            s.V[s.VF] = (byte)(s.V[x] & 0x1);

            s.Pc += 2;
        }

        /// <summary>
        /// Math	Vx=Vy-Vx
        /// Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public void _8XY7(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[s.VF] = s.V[x] > s.V[y] ? (byte)0 : (byte)1;

            s.V[x] = (byte)(s.V[y] - s.V[x]);

            s.Pc += 2;
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
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            s.V[s.VF] = (byte)(s.V[x] >> 7);
            s.V[x] <<= 1;

            s.Pc += 2;
        }

        /// <summary>
        /// Cond	if(Vx!=Vy)
        /// Skips the next instruction if VX doesn't equal VY.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void _9XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;

            if (s.V[x] != s.V[y])
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// MEM
        /// Sets I to the address NNN.
        /// </summary>
        /// <param name="code"></param>
        public void ANNN(short code)
        {
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
            var address = (short)(code & 0x0FFF);
            address += s.V[0];

            s.Pc = address;
        }

        /// <summary>
        /// Rand	Vx=rand()&nn
        /// Sets VX to the result of a bitwise and operation on a random number (Typically: 0 to 255) and nn.
        /// </summary>
        /// <param name="code"></param>
        public void CXNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;

            var rand = new Random().Next(0, 255);

            s.V[x] = (byte)(rand & nn);
            s.Pc += 2;
        }

        /// <summary>
        /// Disp    draw(Vx,Vy,N)
        /// Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels.
        /// Each row of 8 pixels is read as bit-coded starting from memory location I; I value doesn’t
        /// change after the execution of this instruction. As described above,
        /// VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn,
        /// and to 0 if that doesn’t happen
        /// </summary>
        /// <param name="code"></param>
        public void DXYN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            var N = code & 0x000F;

            //Create a temp array to hold the bytes for the sprite
            var spriteData = new byte[N];
            for (var i = 0; i < N; i++)
            {
                spriteData[i] = s.Memory[s.I + i];
            }

            s.Draw(s.V[x], s.V[y], spriteData);
            s.Pc += 2;
        }

        /// <summary>
        /// KeyOp	if(key()==Vx)
        /// Skips the next instruction if the key stored in VX is pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void EX9E(short code)
        {
            var x = (code & 0x0F00) >> 8;

            if (s.Keys[s.V[x]] != 0)
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// KeyOp	if(key()!=Vx)
        /// Skips the next instruction if the key stored in VX isn't pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public void EXA1(short code)
        {
            var x = (code & 0x0F00) >> 8;

            if (s.Keys[s.V[x]] == 0)
            {
                s.Pc += 4;
            }
            else
            {
                s.Pc += 2;
            }
        }

        /// <summary>
        /// Timer	Vx = get_delay()
        /// Sets VX to the value of the delay timer.
        /// </summary>
        /// <param name="code"></param>
        public void FX07(short code)
        {
            var x = (code & 0x0F00) >> 8;

            s.V[x] = s.DelayTimer;

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
            var x = (code & 0x0F00) >> 8;
            //Blocking operation. Waits for a key to be pressed
            var key = s.AnyKeyPressed();
            if (key != 0)
            {
                s.V[x] = s.AnyKeyPressed();
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
            var x = (code & 0xF00) >> 8;

            s.DelayTimer = s.V[x];

            s.Pc += 2;
        }

        /// <summary>
        /// Sound	sound_timer(Vx)
        /// Sets the sound timer to VX.
        /// </summary>
        /// <param name="code"></param>
        public void FX18(short code)
        {
            var x = (code & 0xF00) >> 8;

            s.SoundTimer = s.V[x];

            s.Pc += 2;
        }

        /// <summary>
        /// MEM	I +=Vx
        /// Adds VX to I.
        /// </summary>
        /// <param name="code"></param>
        public void FX1E(short code)
        {
            var x = (code & 0xF00) >> 8;

            s.V[s.VF] = s.I + s.V[(code & 0x0F00) >> 8] > 0xFFF ? (byte)1 : (byte)0;

            s.I += s.V[x];

            s.Pc += 2;
        }

        /// <summary>
        /// MEM	I=sprite_addr[Vx]
        /// Sets I to the location of the sprite for the character in VX.
        /// Characters 0-F (in hexadecimal) are represented by a 4x5 font.
        /// </summary>
        /// <param name="code"></param>
        public void FX29(short code)
        {
            var x = (code & 0xF00) >> 8;

            s.I = (byte)(s.V[x] * 0x5);

            s.Pc += 2;
        }

        /// <summary>
        /// BCD	set_BCD(Vx); *(I+0)=BCD(3); *(I+1)=BCD(2); *(I+2)=BCD(1);
        /// Stores the binary-coded decimal representation of VX,
        /// with the most significant of three digits at the address in I,
        /// the middle digit at I plus 1, and the least significant digit at I plus 2.
        /// (In other words, take the decimal representation of VX,
        /// place the hundreds digit in memory at location in I,
        /// the tens digit at location I+1, and the ones digit at location I+2.)
        /// </summary>
        /// <param name="code"></param>
        public void FX33(short code)
        {
            var x = (code & 0xF00) >> 8;
            var value = s.V[x];
            s.Memory[s.I] = (byte)(value / 100);
            s.Memory[s.I + 1] = (byte)(value / 10 % 10);
            s.Memory[s.I + 2] = (byte)(value % 100 % 10);
            s.Pc += 2;
        }

        /// <summary>
        /// MEM	reg_dump(Vx,&I)
        /// Stores V0 to VX (including VX) in memory starting at address I. I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public void FX55(short code)
        {
            var x = (code & 0xF00) >> 8;

            for (var i = 0; i <= x; i++)
            {
                s.Memory[s.I + i] = s.V[i];
            }

            s.I += (short)(x + 1);

            s.Pc += 2;
        }

        /// <summary>
        /// MEM	reg_load(Vx,&I)
        /// Fills V0 to VX (including VX) with values from memory starting at address I.
        /// I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public void FX65(short code)
        {
            var x = (code & 0xF00) >> 8;

            for (var i = 0; i <= x; i++)
            {
                s.V[i] = s.Memory[s.I + i];
            }

            s.I += (short)(x + 1);

            s.Pc += 2;
        }
    }
}

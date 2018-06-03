using System;

namespace CHIP8Fun.Disassembler
{
    public class OpcodesInstructionSet
    {
        /// <summary>
        /// Call
        /// Calls RCA 1802 program at address NNN. Not necessary for most ROMs.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _0NNN(short code)
        {
            throw new NotImplementedException($"code: {code:x} not implemented");
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public DissasembledLineModel _00E0()
        {
            var dlm = new DissasembledLineModel
            {
                AssemblyCode = "CLS",
                Description = "Clears the screen",
                Opcode = "00E0"
            };

            return dlm;
        }

        /// <summary>
        /// Flow
        /// Returns from a subroutine
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _00EE(short code)
        {
            return CreateDLM("RET", "Returns from a subroutine", code);
        }

        /// <summary>
        /// goto NNN. Jumps to address NNN
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _1NNN(short code)
        {
            var nnn = code & 0xFFF;
            return CreateDLM($"JP {nnn}", $"Jumps to address {nnn}", code);
        }

        /// <summary>
        /// Calls subroutine at NNN
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _2NNN(short code)
        {
            var nnn = code & 0x0FFF;
            return CreateDLM($"CALL {nnn}", $"Calls subroutine at memory address {nnn}", code);
        }

        /// <summary>
        /// Skips the next statement if VX equals nn.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _3XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;
            return CreateDLM($"SE V{x}, {nn}", $"Skips the next statement if V{x} equals {nn}", code);
        }

        /// <summary>
        /// Skips the next istruction if VX doesn't equal nn
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _4XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;
            return CreateDLM($"SNE V{x}, {nn}", $"Skips the next instruction if V{x} doesn't equal {nn}", code);
        }

        /// <summary>
        /// Skips the next instruction if VX equals VY.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _5XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SE V{x}, V{y}", $"Skips the next instruction if V{x} equals V{y}", code);
        }

        /// <summary>
        /// Sets VX to nn
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _6XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = (byte)(code & 0x00FF);
            return CreateDLM($"LD V{x}, {nn}", $"Sets V{x} to {nn}", code);
        }

        /// <summary>
        /// Adds nn to VX. (Carry flag is not changed)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _7XNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;
            return CreateDLM($"ADD V{x}, {nn}", $"Adds {nn} to V{x}", code);
        }

        /// <summary>
        /// Assign
        /// Sets VX to the value of VY
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"LD V{x}, V{y}", $"Sets V{x} to the value of V{y}", code);
        }

        /// <summary>
        /// BitOp Vx=Vx|Vy
        /// Sets VX to VX or VY. (Bitwise OR operation)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY1(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"OR V{x}, V{y}", $"Sets V{x} to V{x} OR V{y} (Bitwise OR operatiorn", code);
        }

        /// <summary>
        /// BitOp Vx=Vx&Vy
        /// Sets VX to VX and VY. (Bitwise AND operation).
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY2(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"AND V{x}, V{y}", $"Sets V{x} to V{x} AND V{y} (Bitwise AND operatiorn", code);
        }

        /// <summary>
        /// BitOp Vx=Vx^Vy
        /// Sets VX to VX xor VY.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY3(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"XOR V{x}, V{y}", $"Sets V{x} to V{x} XOR V{y} (Bitwise XOR operatiorn", code);
        }

        /// <summary>
        /// Math Vx += Vy
        /// Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY4(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"ADD V{x}, V{y}",
                             $"Adds V{y} to V{x}. VF is set to 1 when there's a carry, and to 0 when there isn't.",
                             code);
        }

        /// <summary>
        /// Math Op Vx -= Vy
        /// VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY5(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SUB V{x}, V{y}",
                             $"Subs V{y} to V{x}. VF is set to 0 when there's a borrow, and 1 when there isn't.",
                             code);
        }

        /// <summary>
        /// BitOp	Vx=Vy=Vy>>1
        /// Shifts VY right by one and stores the result to VX (VY remains unchanged).
        /// VF is set to the value of the least significant bit of VY before the shift.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY6(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SHR V{x}, V{y}",
                             $"Shifts V{y} right by one and stores the result to V{x} (V{y} remains unchanged)" +
                             $"VF is set to the value of the least significant bit of V{y} before the shift."
                             , code);
        }

        /// <summary>
        /// Math	Vx=Vy-Vx
        /// Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XY7(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SUBN V{x}, V{y}",
                             $"Sets V{x} to V{y} minus V{x}. " +
                             $"VF is set to 0 when there's a borrow, and 1 when there isn't.",
                             code);
        }

        /// <summary>
        /// BitOp
        /// Shifts VY left by one and copies the result to VX.
        /// VF is set to the value of the most significant bit of VY before the shift.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _8XYE(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SHL V{x}, V{y}",
                             $"Shifts V{y} left by one and copies the result to V{x}." +
                             $"VF is set to the value of the most significant bit of V{y} before the shift.",
                             code);
        }

        /// <summary>
        /// Cond	if(Vx!=Vy)
        /// Skips the next instruction if VX doesn't equal VY.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel _9XY0(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            return CreateDLM($"SNE V{x}, V{y}", $"Skips the next instruction if V{x} doesn't equal V{y}.", code);
        }

        /// <summary>
        /// MEM
        /// Sets I to the address NNN.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel ANNN(short code)
        {
            var nnn = (short)(code & 0x0FFF);
            return CreateDLM($"LD I, {nnn}", $"Sets I to the address {nnn}", code);
        }

        /// <summary>
        /// Flow	PC=V0+NNN
        /// Jumps to the address NNN plus V0.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel BNNN(short code)
        {
            var nnn = (short)(code & 0x0FFF);
            return CreateDLM($"JP V0, {nnn}", $"Jumps to the address {nnn} plus V0.", code);
        }

        /// <summary>
        /// Rand	Vx=rand()&nn
        /// Sets VX to the result of a bitwise and operation on a random number (Typically: 0 to 255) and nn.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel CXNN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var nn = code & 0x00FF;
            return CreateDLM($"RND V{x}, {nn}",
                             $"Sets V{x} to the result of a bitwise and operation on a random number " +
                             $"(Typically: 0 to 255) and {nn}.",
                             code);
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
        public DissasembledLineModel DXYN(short code)
        {
            var x = (code & 0x0F00) >> 8;
            var y = (code & 0x00F0) >> 4;
            var N = code & 0x000F;
            return CreateDLM($"DRW V{x}, V{y}, {N}",
                             $"Draws a sprite at coordinate (V{x}, V{y}) that has a width of 8 pixels and a height of {N} pixels.",
                             code);
        }

        /// <summary>
        /// KeyOp	if(key()==Vx)
        /// Skips the next instruction if the key stored in VX is pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel EX9E(short code)
        {
            var x = (code & 0x0F00) >> 8;
            return CreateDLM($"SKP V{x}", $"Skips the next instruction if the key stored in V{x} is pressed", code);
        }

        /// <summary>
        /// KeyOp	if(key()!=Vx)
        /// Skips the next instruction if the key stored in VX isn't pressed.
        /// (Usually the next instruction is a jump to skip a code block)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel EXA1(short code)
        {
            var x = (code & 0x0F00) >> 8;
            return CreateDLM($"SKNP V{x}", $"Skips the next isntruction if the key stored in V{x} isn't pressed", code);
        }

        /// <summary>
        /// Timer	Vx = get_delay()
        /// Sets VX to the value of the delay timer.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX07(short code)
        {
            var x = (code & 0x0F00) >> 8;
            return CreateDLM($"LD V{x}, DT", $"Sets V{x} to the value of the delay timer", code);
        }

        /// <summary>
        /// KeyOp
        /// A key press is awaited, and then stored in VX.
        /// (Blocking Operation. All instruction halted until next key event)
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX0A(short code)
        {
            var x = (code & 0x0F00) >> 8;
            return CreateDLM($"LD V{x}, K", $"A Key pressed is awaited and then stored in V{x}", code);
        }

        /// <summary>
        /// Timer	delay_timer(Vx)
        /// Sets the delay timer to VX.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX15(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD DT, V{x}", $"Sets the delay timer to V{x}", code);
        }

        /// <summary>
        /// Sound	sound_timer(Vx)
        /// Sets the sound timer to VX.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX18(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD ST, V{x}", $"Sets the sound timer to V{x}", code);
        }

        /// <summary>
        /// MEM	I +=Vx
        /// Adds VX to I.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX1E(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"ADD I, V{x}", $"Adds V{x} to I", code);
        }

        /// <summary>
        /// MEM	I=sprite_addr[Vx]
        /// Sets I to the location of the sprite for the character in VX.
        /// Characters 0-F (in hexadecimal) are represented by a 4x5 font.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX29(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD F, V{x}", $"Sets I to the location of the sprite for the character in V{x}", code);
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
        public DissasembledLineModel FX33(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD B, V{x}",
                             $"Stores the binary-coded decimal representation of VX," +
                             $"with the most significant of three digits at the address in I," +
                             $"the middle digit at I plus 1, and the least significant digit at I plus 2.",
                             code);
        }

        /// <summary>
        /// MEM	reg_dump(Vx,&I)
        /// Stores V0 to VX (including VX) in memory starting at address I. I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX55(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD I, V{x}",
                             $"Stores V0 to VX {x} including V{x}) in memory starting at address I." +
                             $" I is increased by 1 for each value written", code);
        }

        /// <summary>
        /// MEM	reg_load(Vx,&I)
        /// Fills V0 to VX (including VX) with values from memory starting at address I.
        /// I is increased by 1 for each value written.
        /// </summary>
        /// <param name="code"></param>
        public DissasembledLineModel FX65(short code)
        {
            var x = (code & 0xF00) >> 8;
            return CreateDLM($"LD V{x}, I",
                             $"Fills V0 to V{x} (including V{x}) with values from memory starting at address I.", code);
        }

        private DissasembledLineModel CreateDLM(string assembly, string description, short code)
        {
            return new DissasembledLineModel
            {
                AssemblyCode = assembly,
                Description = description,
                Opcode = code.ToString("X")
            };
        }
    }
}

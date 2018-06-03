using System.Collections.Generic;
using System.Diagnostics;

namespace CHIP8Fun.Disassembler
{
    public class DissasembledModel
    {
        private readonly OpcodesInstructionSet instructionsSet;

        public DissasembledModel()
        {
            instructionsSet = new OpcodesInstructionSet();
            LineModels = new List<DissasembledLineModel>();
        }

        public IList<DissasembledLineModel> LineModels {get;}

        /// <summary>
        /// Appends a new decoded lineModel to the list.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="positionInMemory"></param>
        public void AppendLineModel(short opcode, int positionInMemory)
        {
            var dlm = DecodeOpcode(opcode);
            dlm.PositionInMemory = positionInMemory;
            LineModels.Add(dlm);
        }

        private DissasembledLineModel DecodeOpcode(short code)
        {
            switch (code & 0xF000)
            {
                case 0x1000:
                    return instructionsSet._1NNN(code);
                case 0x2000:
                    return instructionsSet._2NNN(code);

                case 0x3000:
                    return instructionsSet._3XNN(code);

                case 0x4000:
                    return instructionsSet._4XNN(code);

                case 0x5000:
                    return instructionsSet._5XY0(code);

                case 0x6000:
                    return instructionsSet._6XNN(code);

                case 0x7000:
                    return instructionsSet._7XNN(code);

                case 0x8000:
                {
                    switch (code & 0x000F)
                    {
                        case 0x0000:
                            return instructionsSet._8XY0(code);

                        case 0x0001:
                            return instructionsSet._8XY1(code);

                        case 0x0002:
                            return instructionsSet._8XY2(code);

                        case 0x0003:
                            return instructionsSet._8XY3(code);

                        case 0x0004:
                            return instructionsSet._8XY4(code);

                        case 0x0005:
                            return instructionsSet._8XY5(code);

                        case 0x0006:
                            return instructionsSet._8XY6(code);

                        case 0x0007:
                            return instructionsSet._8XY7(code);

                        case 0x000E:
                            return instructionsSet._8XYE(code);

                        default:
                            Debug.WriteLine($"Unknown opcode [0x0000]: {code:X}");
                            return default;
                    }
                }

                case 0x9000:
                    return instructionsSet._9XY0(code);

                case 0xA000:
                    return instructionsSet.ANNN(code);

                case 0xB000:
                    return instructionsSet.BNNN(code);

                case 0xC000:
                    return instructionsSet.CXNN(code);

                case 0xD000:
                    return instructionsSet.DXYN(code);

                case 0xE000:
                {
                    switch (code & 0x000F)
                    {
                        case 0x000E:
                            return instructionsSet.EX9E(code);

                        case 0x0001:
                            return instructionsSet.EXA1(code);

                        default:
                            Debug.WriteLine($"Unknown opcode [0x0000]: {code:X}");
                            return default;
                    }
                }
                case 0xF000: // FX0A: Wait for key press, store key pressed in VX
                    switch (code & 0x0FF)
                    {
                        case 0x0007:
                            return instructionsSet.FX07(code);

                        case 0x000A:
                            return instructionsSet.FX0A(code);

                        case 0x0015:
                            return instructionsSet.FX15(code);

                        case 0x0018:
                            return instructionsSet.FX18(code);

                        case 0x001E:
                            return instructionsSet.FX1E(code);

                        case 0x0029:
                            return instructionsSet.FX29(code);

                        case 0x0033:
                            return instructionsSet.FX33(code);

                        case 0x0055:
                            return instructionsSet.FX55(code);

                        case 0x0065:
                            return instructionsSet.FX65(code);

                        default:
                            Debug.WriteLine($"Unknown opcode [0x0000]: {code:X}");
                            return default;
                    }


                //In some cases we can not rely solely on the first four bits to see what the opcode means.
                //For example, 0x00E0 and 0x00EE both start with 0x0.
                //In this case we add an additional switch and compare the last four bits:
                case 0x0000:
                {
                    switch (code & 0x000F)
                    {
                        case 0x0000: // 0x00E0: Clears the screen
                            return instructionsSet._00E0();

                        case 0x000E: // 0x00EE: Returns from subroutine
                            return instructionsSet._00EE(code);

                        default:
                            Debug.WriteLine($"Unknown opcode [0x0000]: {code:X}");
                            return default;
                    }
                }


                default:
                    Debug.WriteLine($"Unknown opcode: {code:X}");
                    return default;
                    ;
            }
        }
    }
}

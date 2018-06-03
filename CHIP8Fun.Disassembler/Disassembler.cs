using System.Text;

namespace CHIP8Fun.Disassembler
{
    public class Disassembler
    {
        /// <summary>
        /// Source
        /// </summary>
        private readonly byte[] opcodes;

        public Disassembler(byte[] opcodes)
        {
            this.opcodes = opcodes;
        }

        public DissasembledModel Dissasemble()
        {
            var model = new DissasembledModel();
            for (var i = 0; i < opcodes.Length-1; i+=2)
            {
                var opcode = (short)((opcodes[i] << 8) | opcodes[i + 1]);
                model.AppendLineModel(opcode, 512 + i);
            }

            return model;
        }
    }
}

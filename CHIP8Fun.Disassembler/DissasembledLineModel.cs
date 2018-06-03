namespace CHIP8Fun.Disassembler
{
    public struct DissasembledLineModel
    {
        public string Opcode {get; set;}
        public string AssemblyCode {get; set;}
        public string Description {get; set;}
        public int PositionInMemory {get; set;}
    }
}

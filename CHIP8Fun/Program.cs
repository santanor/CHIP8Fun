namespace CHIP8Fun
{
    internal class Program
    {
        private static CHIP8System chip8;

        /// <summary>
        /// Emulator Loop
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Set up render system and register input callbacks
            //SetupGraphics();
            //SetupInput();

            // Initialize the Chip8 system and load the game into the memory
            chip8 = new CHIP8System();
            chip8.LoadProgram("Clock.ch8");

            // Emulation loop
            while (true)
            {
                // Emulate one cycle
                chip8.EmulateCycle();

                // If the draw flag is set, update the screen
                //if (chip8.drawFlag)
                //{
                //    drawGraphics();
                //}


                // Store key press state (Press and Release)
                //chip8.setKeys();
            }
        }
    }
}

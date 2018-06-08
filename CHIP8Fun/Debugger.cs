using System.Threading;
using System.Windows.Input;

namespace CHIP8Fun
{
    public class Debugger
    {
        private readonly Emulator emulator;
        private bool isDebuggerRunning;
        public bool Paused {get; set;}
        public bool StepOver {get; set;}

        /// <summary>
        /// Registers the emulator on create
        /// </summary>
        /// <param name="emulator"></param>
        public Debugger(Emulator emulator)
        {
            this.emulator = emulator;
            this.emulator.OnUIKeyDown += OnUIKeyDown;
            this.Paused = true;
        }

        /// <summary>
        /// Pauses the execution
        /// </summary>
        private void Pause()
        {
            isDebuggerRunning = true;
            Paused = true;
        }

        private void Step()
        {
            Paused = false;
            StepOver = true;
        }

        /// <summary>
        /// Resumes the execution
        /// </summary>
        private void Continue()
        {
            Paused = false;
            isDebuggerRunning = false;
        }

        /// <summary>
        /// Runs the main program in a debug enviroment which can pause and step over the instructions
        /// </summary>
        public void Run()
        {
            while (true)
            {
                if (Paused)
                {
                    //Just wait a little bit to free up the cpu
                    Thread.Sleep(10);
                }
                else
                {
                    // Emulate one cycle
                    emulator.Chip8.EmulateCycle();
                    emulator.TryPresentFrame();
                    emulator.TryUpdateUiData();

                    //If the execution was done by a step, reset the values so it works properly
                    if (StepOver)
                    {
                        StepOver = false;
                        Paused = true;
                    }
                }
            }
        }

        private void OnUIKeyDown(KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.F9:
                    Step();
                    break;
                case Key.F5 when Paused:
                    Continue();
                    break;
                case Key.F5:
                    Pause();
                    break;
            }
        }
    }
}

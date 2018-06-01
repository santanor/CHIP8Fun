using System.Threading;
using System.Windows.Input;

namespace CHIP8Fun
{
    public class Debugger
    {
        private readonly Emulator emulator;
        private bool isDebuggerRunning;
        private bool paused;
        private bool stepOver;

        /// <summary>
        /// Registers the emulator on create
        /// </summary>
        /// <param name="emulator"></param>
        public Debugger(Emulator emulator)
        {
            this.emulator = emulator;
            this.emulator.OnUIKeyDown += OnUIKeyDown;
        }

        /// <summary>
        /// Pauses the execution
        /// </summary>
        private void Pause()
        {
            isDebuggerRunning = true;
            paused = true;
        }

        private void Step()
        {
            paused = false;
            stepOver = true;
        }

        /// <summary>
        /// Resumes the execution
        /// </summary>
        private void Continue()
        {
            paused = false;
            isDebuggerRunning = false;
        }

        /// <summary>
        /// Runs the main program in a debug enviroment which can pause and step over the instructions
        /// </summary>
        public void Run()
        {
            while (true)
            {
                if (paused)
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
                    if (stepOver)
                    {
                        stepOver = false;
                        paused = true;
                    }
                }
            }
        }

        private void OnUIKeyDown(KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                //Apparently this is the value for F10
                case Key.System:
                    Step();
                    break;
                case Key.F5 when paused:
                    Continue();
                    break;
                case Key.F5:
                    Pause();
                    break;
            }
        }
    }
}

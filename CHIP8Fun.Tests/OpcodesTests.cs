using System.Drawing;
using NUnit.Framework;

namespace CHIP8Fun.Tests
{
    [TestFixture]
    public class OpcodesTests
    {
        private CHIP8System chip;
        private Bitmap bitmap;

        [Test]
        public void Test00E0()
        {
            Assert.IsNotNull(chip.Gfx);
            Assert.IsTrue(ScreenClear());
            chip.Gfx[0, 0] = 1;
            Assert.IsFalse(ScreenClear());
            chip.ExecuteOpcode(224);
            Assert.IsTrue(ScreenClear());

            bool ScreenClear()
            {
                var clear = true;
                foreach (var b in chip.Gfx)
                {
                    clear = clear & (b == 0);
                }
                return clear;
            }
        }

        [SetUp]
        public void InitializeEmulator()
        {
            bitmap = new Bitmap(64,32);
            chip = new CHIP8System(bitmap);
        }
    }
}

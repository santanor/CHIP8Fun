using System.Drawing;
using Color = System.Drawing.Color;

namespace CHIP8Fun
{
    public class GPU
    {
        private Bitmap backingImage;

        public GPU(Bitmap image, int width, int height)
        {
            backingImage = image;
        }

        public void SetPixel(int x, int y, Color c)
        {
            backingImage.SetPixel(x, y, c);
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using SDPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.IO;

namespace RenderToBmp
{
    static class Program
    {
        static void Main()
        {
            var mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1); 
            var win = new OpenTK.GameWindow(640, 480, mode, "", OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 3, 0, GraphicsContextFlags.Default);
            win.Visible = false;
            win.MakeCurrent();
            /* START OF YOUR ACTUAL GL CODE */
            GL.ClearColor(0.7f, 0.7f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            /* END OF YOUR ACTUAL GL CODE */
            GL.Flush();
            using (var bmp = new Bitmap(640, 480, SDPixelFormat.Format32bppArgb))
            {
                var mem = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.WriteOnly, SDPixelFormat.Format32bppArgb);
                GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
                GL.ReadPixels(0, 0, 640, 480, PixelFormat.Bgra, PixelType.UnsignedByte, mem.Scan0);
                bmp.UnlockBits(mem);
                bmp.Save(@"Test.png", ImageFormat.Png);
            }

            bool refex = false;
            bool tesex = false;

            Console.WriteLine(Directory.GetCurrentDirectory());

            if (File.Exists(@"Reference.png"))
            {
                Console.WriteLine("Reference exists");
                refex = true;
            }

            if (File.Exists("Test.png"))
            {
                Console.WriteLine("Test exists");
                tesex = true;
            }

            if (refex & tesex)
            {
                var referenceIm = new Bitmap(@"Reference.png");
                var testIm = new Bitmap("Test.png");

                float percent = compareImage(referenceIm, testIm);
                Console.WriteLine("Result: " + percent);
            }
        }

        private static float compareImage(Bitmap referenceIm, Bitmap testIm)
        {
            var count = 0;

            for (int x = 0; x < System.Math.Min(referenceIm.Width, testIm.Width); x++)
            {
                for (int y = 0; y < System.Math.Min(referenceIm.Height, testIm.Height); y++)
                {
                    if (!testIm.GetPixel(x, y).Equals(referenceIm.GetPixel(x, y)))
                        count++;
                }
            }

            return 1 - ((float)count / (referenceIm.Height * referenceIm.Width));
        }
    }
}
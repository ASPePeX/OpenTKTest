using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using SDPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.IO;
using Common;

namespace RenderToBmp
{
    static class Test3
    {
        static void Main()
        {
            var mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1); 
            var win = new OpenTK.GameWindow(640, 480, mode, "", OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 3, 0, GraphicsContextFlags.Default);
            win.Visible = false;
            win.MakeCurrent();
            /* START OF YOUR ACTUAL GL CODE */
            GL.ClearColor(0.6f, 1f, 0.6f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            /* END OF YOUR ACTUAL GL CODE */
            GL.Flush();
            using (var bmp = new Bitmap(640, 480, SDPixelFormat.Format32bppArgb))
            {
                var mem = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.WriteOnly, SDPixelFormat.Format32bppArgb);
                GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
                GL.ReadPixels(0, 0, 640, 480, PixelFormat.Bgra, PixelType.UnsignedByte, mem.Scan0);
                bmp.UnlockBits(mem);
                bmp.Save(@"Test3.png", ImageFormat.Png);
            }

            string refimg = Path.Combine(Directory.GetCurrentDirectory(), "Test3", "Reference.png");
            string tesimg = Path.Combine(Directory.GetCurrentDirectory(), "Test3.png");

            Helper.CompareTestImages(refimg, tesimg);
        }
    }
}
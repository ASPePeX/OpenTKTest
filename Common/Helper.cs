using System;
using System.Drawing;
using System.IO;

namespace Common
{
    public static class Helper
    {
        public static void CompareTestImages(string path1, string path2)
        {
            if (File.Exists(path1) && File.Exists(path2))
            {
                var img1 = new Bitmap(path1);
                var img2 = new Bitmap(path2);

                float percent = compareImage(img1, img2);
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

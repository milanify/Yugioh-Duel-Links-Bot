using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yugioh_Duel_Links_Bot
{
    // Handles screenshots and  graphics
    public class BitmapGraphics
    {
        private static Graphics graphicToDelete;
        private static Dictionary<Bitmap, Graphics> bitmapGraphicDict = new Dictionary<Bitmap, Graphics>();

        // Store snapshot, able to draw screen in bitmap, copy from screen into bitmap
        public static Bitmap Screenshot()
        {
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmpScreenshot);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            bitmapGraphicDict.Add(bmpScreenshot, g);
            return bmpScreenshot;
        }

        // Dipose of specified graphic by checking dictionary
        public static void disposeGraphic(Bitmap b)
        {
            graphicToDelete = bitmapGraphicDict[b];
            b.Dispose();
            graphicToDelete.Dispose();
        }

        // Find image (needle) in screenshot of desktop (haystack)
        public static bool FindBitMap(Bitmap bmpNeedle, Bitmap bmpHaystack, out Point location)
        {
            for (int outerX = 0; outerX < bmpHaystack.Width - bmpNeedle.Width; outerX++)
            {
                for (int outerY = 0; outerY < bmpHaystack.Height - bmpNeedle.Height; outerY++)
                {
                    for (int innerX = 0; innerX < bmpNeedle.Width; innerX++)
                    {
                        for (int innerY = 0; innerY < bmpNeedle.Height; innerY++)
                        {
                            Color cNeedle = bmpNeedle.GetPixel(innerX, innerY);
                            Color cHaystack = bmpHaystack.GetPixel(innerX + outerX, innerY + outerY);

                            if (cNeedle.R != cHaystack.R || cNeedle.G != cHaystack.G || cNeedle.B != cHaystack.B)
                            {
                                goto notFound;
                            }
                        }
                    }
                    location = new Point(outerX, outerY);
                    return true;

                notFound:
                    continue;
                }
            }
            location = Point.Empty;
            return false;
        }
    }
}

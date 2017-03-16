using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yugioh_Duel_Links_Bot
{
    // Handles where the cursor will go
    class CursorPath
    {
        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool block);
        private static Bitmap initialScrenshot = BitmapGraphics.Screenshot();
        private static Point location;
        private static Point startPoint;

        // Waits for a second and then checks if duel button exists on screen
        private static bool waitAndCheckForDuelButton()
        {
            Thread.Sleep(1000);
            return Dueling.isDuelButton(location);                                  
        }

        // Sets an anchor image as a reference location for the start point
        public static void setAnchorImage(String resourceName)
        {
            Bitmap panels = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourceName);
            BitmapGraphics.FindBitMap(panels, initialScrenshot, out location);          
            Cursor.Position = location;                       
        }

        // Sets start point
        public static void setInitialLocation(int x_shift, int y_shift)
        {
            location = new Point(Cursor.Position.X + x_shift, Cursor.Position.Y + y_shift);
            Cursor.Position = location;
            startPoint = location;
        }

        // Moves cursor to another location
        private static void setCursorLocation(int x_shift, int y_shift)
        {
            location = new Point(Cursor.Position.X + x_shift, Cursor.Position.Y + y_shift);
            Cursor.Position = location;
        }

        // Clicks for a specified number of times while moving in a specified direction
        private static void clickerLoop(int iterations, int x_shift, int y_shift)
        {
            for (int i = 0; i < iterations; i++)
            {
                location = new Point(Cursor.Position.X + x_shift, Cursor.Position.Y + y_shift);
                Cursor.Position = location;
                Thread.Sleep(100);
                MouseActions.MouseClick();
            }
        }

        // Duels at gate location
        public static void duelAtGate()
        {
            setAnchorImage("bmpPanel2");
            setInitialLocation(-175, -140);
            clickerLoop(6, 50, 0);
            Thread.Sleep(2000);

            setCursorLocation(0, -80);
            clickerLoop(5, -30, 0);

            int[] setCursorX = { 0, 0, 0, 0 };
            int[] setCursorY = { -100, -70, -50, 0 };
            int[] numberOfLoops = { 10, 10, 10, 2 };
            int[] shiftX = { 25, -25, 12, 0 };
            int[] shiftY = { 0, 0, 0, 0 };
            int i = 0;

            while (!waitAndCheckForDuelButton() && i < 3)
            {
                setCursorLocation(setCursorX[i], setCursorY[i]);
                clickerLoop(numberOfLoops[i], shiftX[i], shiftY[i]);
                i++;                
            }

            Dueling.locateDuelButton();
        }

        // Duels at PVP Arena location
        public static void duelAtPVPArena()
        {
            Thread.Sleep(2000);
            Cursor.Position = startPoint;
            clickerLoop(12, 25, 0);
            Thread.Sleep(2000);

            int[] setCursorX = { 0, 0, 0, 0 };
            int[] setCursorY = { -80, -80, -70, -5 };
            int[] numberOfLoops = { 4, 4, 8, 8 };
            int[] shiftX = { -50, 55, -25, 20 };
            int[] shiftY = { 0, 0, 0, 0 };
            int i = 0;

            while (!waitAndCheckForDuelButton() && i < 4)
            {
                setCursorLocation(setCursorX[i], setCursorY[i]);
                clickerLoop(numberOfLoops[i], shiftX[i], shiftY[i]);
                i++;
            }

            Dueling.locateDuelButton();
        }

        // Duels at Shop location
        public static void duelAtShop()
        {
            Thread.Sleep(2000);
            Cursor.Position = startPoint;
            setCursorLocation(0, -50);
            clickerLoop(5, 25, 0);
            Thread.Sleep(2000);

            int[] setCursorX = { 160, 0, 0, 0, 0 };
            int[] setCursorY = { 0, -80, -70, -240, 400 };
            int[] numberOfLoops = { 3, 10, 10, 11, 1 };
            int[] shiftX = { 15, -32, 42, -40, 0 };
            int[] shiftY = { 0, 0, 0, 0, 0 };
            int i = 0;

            while (!waitAndCheckForDuelButton() && i < 5)
            {
                setCursorLocation(setCursorX[i], setCursorY[i]);
                clickerLoop(numberOfLoops[i], shiftX[i], shiftY[i]);
                i++;
            }

            Dueling.locateDuelButton();
        }
        
        // Duels at Card Studio
        public static void duelAtCardStudio()
        {
            Thread.Sleep(2000);
            Cursor.Position = startPoint;
            setCursorLocation(0, 30);
            clickerLoop(11, 25, 0);
            Thread.Sleep(2000);

            int[] setCursorX = { 0, 0, 0, 0, 0 };
            int[] setCursorY = { -80, -50, -60, -19, 0 };
            int[] numberOfLoops = { 8, 10, 8, 10, 12 };
            int[] shiftX = { -25, 25, -25, 25, -25 };
            int[] shiftY = { 0, 0, 0, 0, 0 };
            int i = 0;

            while (!waitAndCheckForDuelButton() && i < 5)
            {
                setCursorLocation(setCursorX[i], setCursorY[i]);
                clickerLoop(numberOfLoops[i], shiftX[i], shiftY[i]);
                i++;
            }

            Thread.Sleep(1000);
            Dueling.locateDuelButton();
        }

        // Blocks all input from the user
        public static void FreezeInput()
        {
            BlockInput(true);
        }

        // Enables input from the user
        public static void ThawInput()
        {
            BlockInput(false);
        }
    }
}

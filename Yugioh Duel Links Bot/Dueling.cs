using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Yugioh_Duel_Links_Bot
{
    // Handles dueling actions
    class Dueling
    {
        private static Bitmap tempScreenshot;
        private static bool logButtonExists;
        private static bool duelButtonExists;
        private static Point location;
        public static bool isDuelOver = false;
        private static frmMain frm;

        // Constructor to reference form's controls
        public Dueling(frmMain form)
        {
            frm = form;
        }

        // Check if duel button exists, or if 'OK' dialog pops up and returns cursor to origina location after it clicks it
        public static bool isDuelButton(Point originalLocation)
        {
            tempScreenshot = BitmapGraphics.Screenshot();
            duelButtonExists = BitmapGraphics.FindBitMap(Properties.Resources.bmpBackArrow, tempScreenshot, out location);            
                    
            if (duelButtonExists)
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
                return duelButtonExists;
            } else if (BitmapGraphics.FindBitMap(Properties.Resources.bmpDuelButton, tempScreenshot, out location))
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
                return true;
            } else if (BitmapGraphics.FindBitMap(Properties.Resources.bmpOKButton, tempScreenshot, out location))
            {
                Cursor.Position = location;
                MouseActions.MouseClick();
                BitmapGraphics.disposeGraphic(tempScreenshot);
                Cursor.Position = originalLocation;
                return false;
            }
            else
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
                return false;
            }           
        }

        // Find duel button
        public static void locateDuelButton()
        {
            tempScreenshot = BitmapGraphics.Screenshot();
            duelButtonExists = BitmapGraphics.FindBitMap(Properties.Resources.bmpBackArrow, tempScreenshot, out location);

            frm.displayText("Trying to find duel button.");

            if (duelButtonExists)
            {
                Cursor.Position = location;
                location = new Point(Cursor.Position.X + 230, Cursor.Position.Y + -65);
                clickDuelButton();
                waitAndFindAutoDuelButton();
                checkIfDuelEnded(5000);

            } else if (BitmapGraphics.FindBitMap(Properties.Resources.bmpDuelButton, tempScreenshot, out location))
            {
                clickDuelButton();
                waitAndFindAutoDuelButton();
                checkIfDuelEnded(5000);
            } else
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
            }
        }

        // Click duel button
        private static void clickDuelButton()
        {
            Cursor.Position = location;
            MouseActions.MouseClick();
            Thread.Sleep(2000);
            MouseActions.MouseClick();
            frm.displayText("Clicked on duel button.");
            BitmapGraphics.disposeGraphic(tempScreenshot);
        }

        // Waits for duel to start and then finds auto duel button
        private static void waitAndFindAutoDuelButton()
        {
            Thread.Sleep(33000);
            locateAutoDuelButton();
        }

        // Finds auto duel button
        public static void locateAutoDuelButton()
        {
            tempScreenshot = BitmapGraphics.Screenshot();
            MouseActions.MouseClick();
            frm.displayText("Trying to find auto-duel button.");

            if (BitmapGraphics.FindBitMap(Properties.Resources.bmpAutoDuelButton, tempScreenshot, out location))
            {
                clickAutoDuelButton();
            } else if (BitmapGraphics.FindBitMap(Properties.Resources.bmpAutoDuelButton2, tempScreenshot, out location))
            {
                clickAutoDuelButton();
                
            } else
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
                MouseActions.MouseClick();
                Thread.Sleep(3000);
                locateAutoDuelButton();
            }
        }

        // Clicks auto duel button
        private static void clickAutoDuelButton()
        {
            Cursor.Position = location;
            MouseActions.MouseClick();
            BitmapGraphics.disposeGraphic(tempScreenshot);
            frm.displayText("Clicked on auto-duel.");
            CursorPath.ThawInput();
            frm.changeButtonColor(Color.LightGreen);
            Thread.Sleep(30000);
        }

        // Checks if duel is finished, with a parameter to extend time between checks
        public static void checkIfDuelEnded(int timeBetweenChecks)
        {
            tempScreenshot = BitmapGraphics.Screenshot();
            logButtonExists = BitmapGraphics.FindBitMap(Properties.Resources.bmpLogButton, tempScreenshot, out location);
            int timeToIncreaseBetweenChecks = 0;
                        
            if (logButtonExists)
            {
                isDuelOver = true;
                frm.displayText("Ending duel.");
                CursorPath.FreezeInput();
                frm.changeButtonColor(Color.Red);

                BitmapGraphics.FindBitMap(Properties.Resources.bmpLogButton, tempScreenshot, out location);
                Cursor.Position = location;

                location = new Point(Cursor.Position.X - 150, Cursor.Position.Y + 120);
                Cursor.Position = location;
                MouseActions.MouseClick();
                BitmapGraphics.disposeGraphic(tempScreenshot);

                clickThroughDialogs();
            }
            else if (!logButtonExists)
            {
                frm.displayText("Checking if duel ended.");
                Thread.Sleep(timeBetweenChecks);
                BitmapGraphics.disposeGraphic(tempScreenshot);
                checkIfDuelEnded(timeBetweenChecks + timeToIncreaseBetweenChecks);
            }
        }

        // Click through all dialogs at the end of duel
        private static void clickThroughDialogs()
        {
            clickThroughLevelRewards();
            checkForAdditionalChatDialog();
        }

        // Click through initial rewards
        private static void clickThroughLevelRewards()
        {
            for (int i = 0; i < 7; i++)
            {
                Thread.Sleep(3000);
                MouseActions.MouseClick();
            }

            Thread.Sleep(3000);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(3000);
                MouseActions.MouseClick();
            }
        }

        // Check for additional character dialog after all clicks
        private static void checkForAdditionalChatDialog()
        {
            tempScreenshot = BitmapGraphics.Screenshot();
            bool secondDialog = BitmapGraphics.FindBitMap(Properties.Resources.bmpBeforeAfterDuel, tempScreenshot, out location);
            if (secondDialog)
            {
                Cursor.Position = location;
                MouseActions.MouseClick();
                BitmapGraphics.disposeGraphic(tempScreenshot);
                Thread.Sleep(4000);
            }
            else
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
            }
        }

        // Chcecks for an "OK" button that pops up after all character dialogs
        private static void checkForFinalOKButton()
        {
            frm.displayText("Waiting for possible additional dialogs...");
            tempScreenshot = BitmapGraphics.Screenshot();
            bool finalOk = BitmapGraphics.FindBitMap(Properties.Resources.bmpCampReward, tempScreenshot, out location);
            if (finalOk)
            {
                Cursor.Position = location;
                MouseActions.MouseClick();
                BitmapGraphics.disposeGraphic(tempScreenshot);
                Thread.Sleep(2000);
            }
            else
            {
                BitmapGraphics.disposeGraphic(tempScreenshot);
            }
        }
    }
}

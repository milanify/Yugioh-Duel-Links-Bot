using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Yugioh_Duel_Links_Bot
{
    public partial class frmMain : Form
    {
        private static IntPtr hWnd;     
        delegate void delWithNoParameters();
        private bool skipCheckbox1 = false;
        private bool skipCheckbox2 = false;
        private bool skipCheckbox3 = false;
        private bool skipCheckbox4 = false;

        // Loads main form with specified parameters
        public frmMain()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Yugioh_Duel_Links_Bot.Properties.Resources.yugiohlogo;
            textBox1.Text = "5";
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatStyle = FlatStyle.Flat;
            new Dueling(this);
        }

        // When button is clicked, dueling begins
        private void button1_Click(object sender, EventArgs e)
        {
            displayText("Started.");
            CursorPath.FreezeInput();
            changeButtonColor(Color.Red);
            Bitmap bmpScreenshot = BitmapGraphics.Screenshot();
            Bitmap initialScrenshot = BitmapGraphics.Screenshot();
            Point location;
            
            CursorPath.setAnchorImage("bmpPanel2");
            CursorPath.setInitialLocation(-175, -140);

            /*var images = Properties.Resources.ResourceManager
                       .GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true)
                       .Cast<System.Collections.DictionaryEntry>()
                       .Where(x => x.Value.GetType() == typeof(Bitmap))
                       .Select(x => x.Key.ToString())
                       .ToList();*/

            String[] panels = new String[4] { "bmpPanel2", "bmpPanel3", "bmpPanel4", "bmpPanel1" };
            bool[] skipPanels = new bool[4] { skipCheckbox1, skipCheckbox2, skipCheckbox3, skipCheckbox4 };
            Delegate[] findDuelistMethods = new Delegate[4];
            findDuelistMethods[0] = new delWithNoParameters(CursorPath.duelAtGate);
            findDuelistMethods[1] = new delWithNoParameters(CursorPath.duelAtPVPArena);
            findDuelistMethods[2] = new delWithNoParameters(CursorPath.duelAtShop);
            findDuelistMethods[3] = new delWithNoParameters(CursorPath.duelAtCardStudio);

            int j = 0;
            int numberOfTimesToCheck = Int32.Parse(textBox1.Text);

            foreach (String p in panels)
            {
                Bitmap duelingZones = (Bitmap)Properties.Resources.ResourceManager.GetObject(p);

                for (int i = 0; i < numberOfTimesToCheck; i++)
                {
                    if (skipPanels[j])
                    {
                        skipPanels[j] = false;
                        break;
                    }
                    findDuelistMethods[j].DynamicInvoke();
                }
                initialScrenshot = BitmapGraphics.Screenshot();
                BitmapGraphics.FindBitMap(duelingZones, initialScrenshot, out location);
                Cursor.Position = location;
                MouseActions.MouseClick();
                BitmapGraphics.disposeGraphic(initialScrenshot);
                j++;
            }
            CursorPath.ThawInput();
            changeButtonColor(Color.LightGreen);
            displayText("Finished.");
        }

        // Displays specified text to log on a new line
        public void displayText(string text)
        {
            textBox2.AppendText(text + Environment.NewLine);
            this.Refresh();
        }

        // Gets window handle of specified window
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hwnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }       

        // For skipping certain locations
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            skipCheckbox1 = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            skipCheckbox2 = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            skipCheckbox3 = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            skipCheckbox4 = true;
        }

        // Log settings
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.ScrollBars = ScrollBars.Both;
            textBox2.WordWrap = true;
        }

        // Changes color of quit button to let user know whether or not they can click
        public void changeButtonColor(Color c)
        {
            button2.BackColor = c;
            button2.Refresh();
        }

        // Pressing the quit button exits the application
        public void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

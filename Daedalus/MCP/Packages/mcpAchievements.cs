using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;

namespace Daedalus.MCP.Packages
{
    class mcpAchievements : MCPPackage
    {
        MCPHandler handler;
        public mcpAchievements(MCPHandler handler)
        {
            this.handler = handler;
        }
        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-be-obia-achievements"; }
        }

        public string minVer
        {
            get { return "1.0"; }
        }

        public string maxVer
        {
            get { return "1.0"; }
        }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-be-obia-achievements-achievement")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "name", "description", "icon")) // Score and  aren't useful yet.
                    return;
                new AchievementPopupDialogue(KeyVals["name"] as string, KeyVals["description"] as string, KeyVals["icon"] as string).Show();
            }
        }

        public void Negotiated()
        {
            
        }

        public void Disconnected()
        {
            
        }

        #endregion
        private class AchievementPopupDialogue : Form
        {
            private string AchName;
            private string AchDesc;
            private Image icon;

            Font FontName = new System.Drawing.Font("Segoe WP Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font FontDesc = new System.Drawing.Font("Segoe WP SemiLight", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Timer timer = new Timer();
            WebClient wc;
            bool DownloadingImage = false;

            public AchievementPopupDialogue(string name, string desc, string icon)
            {
                this.AchName = name;
                this.AchDesc = desc;
                if (!string.IsNullOrEmpty(icon) && icon != "\"\"")
                {
                    wc = new WebClient();
                    wc.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                    try
                    {
                        if (!Directory.Exists("Cache"))
                            Directory.CreateDirectory("Cache");
                        wc.DownloadFileAsync(new Uri(icon), Path.Combine("Cache", Path.GetFileName(icon)), Path.Combine("Cache", Path.GetFileName(icon)));
                        DownloadingImage = true;
                    }
                    catch (WebException) { }
                    catch (Exception) { }
                }

                this.Height = 69;
                this.Width = 300;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
                this.ShowInTaskbar = false;

                this.Top = Screen.PrimaryScreen.WorkingArea.Bottom;
                this.Left = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
                timer.Interval = 5;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                this.TopMost = true;
            }

            void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                try
                {
                    this.icon = Image.FromFile(e.UserState as string);
                }
                catch (OutOfMemoryException) { } // GDI+ has some weird ideas on what an OOM exception actually is.
                this.Invalidate();
                DownloadingImage = false;
                timeshown = DateTime.UtcNow;
            }

            int stage = 1;
            DateTime timeshown = DateTime.UtcNow;
            void timer_Tick(object sender, EventArgs e)
            {
                int bottom = Screen.PrimaryScreen.WorkingArea.Bottom;
                foreach (Form f in Application.OpenForms)
                {
                    if (f is AchievementPopupDialogue)
                    {
                        if (f == this)
                            break;
                        bottom = f.Top;
                    }
                }
                if (DownloadingImage)
                    return;
                if (stage == 1)
                    if (this.Bottom > bottom)
                        this.Top -= 1;
                    else
                        stage = 2;
                else if (stage == 2)
                    if (DateTime.UtcNow.Subtract(timeshown).TotalSeconds > 20)
                        stage = 3;
                    else 
                    {
                        if (this.Bottom <= bottom)
                            this.Top += 1; // another one closed, so go back down
                    }
                else if (stage == 3)
                    if (this.Top > Screen.PrimaryScreen.WorkingArea.Bottom)
                    {
                        timer.Stop();
                        timer.Dispose();
                        if (this.icon != null)
                            this.icon.Dispose();
                        this.Close();
                        this.Dispose();
                    }
                    else
                        this.Top += 1;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                int textstart = 5;
                if (icon != null)
                {
                    e.Graphics.DrawImage(icon, new Rectangle(5, 5, 64, 64));
                    textstart = 69;
                }
                e.Graphics.DrawString(AchName, FontName, Brushes.Black, textstart, 5);
                e.Graphics.DrawString(AchDesc, FontDesc, Brushes.Black, textstart, 30);
            }
            
        }
    }
}

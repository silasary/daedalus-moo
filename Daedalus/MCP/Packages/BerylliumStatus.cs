using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Daedalus.MCP.Packages
{
    class BerylliumStatus : MCPPackage
    {
        List<TrayIcon> TrayIcons = new List<TrayIcon>();
        internal ImageList TrayImage = new ImageList();
        Connection connection;
        public BerylliumStatus(MCPHandler handler)
        {
            this.connection = handler.CurrentConnection;
        }

        public string PackageName
        {
            get { return "dns-net-beryllium-status"; }
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
            if (command == "dns-net-beryllium-status-msg_force")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "text", "cmd"))
                    return;
                connection.SetStatus(KeyVals["text"], new Chiroptera.Win.Paragraph.MetaData(0, System.Drawing.Color.Black, System.Drawing.Color.White) { linkurl = KeyVals["cmd"] });
            }
            else if (command == "dns-net-beryllium-status-ico_clr")
            {
                // TODO:  Clear them from the window.
                this.TrayIcons = new List<TrayIcon>();
            }
            else if (command == "dns-net-beryllium-status-ico_add" || command == "dns-net-beryllium-status-ico_upd")
            {
                TrayIcon icon;
                if (command == "dns-net-beryllium-status-ico_add")
                {
                    this.TrayIcons.Add(icon = new TrayIcon(this));
                    icon.Size = new System.Drawing.Size(16, 16);
                }
                else
                    icon = this.TrayIcons[int.Parse(KeyVals["index"])];                
                if (KeyVals.ContainsKey("img0"))
                    icon.Icon = Math.Abs(int.Parse(KeyVals["img0"]));
                else if (KeyVals.ContainsKey("img"))
                    icon.Icon = Math.Abs(int.Parse(KeyVals["img"]));
                else if (icon.Icon == 0)
                    return;
                icon.OnIcon = icon.Icon;
                if (KeyVals.ContainsKey("img1"))
                    icon.OffIcon = Math.Abs(int.Parse(KeyVals["img1"]));
                if (KeyVals.ContainsKey("mode"))
                    icon.Mode = (IconMode)Enum.Parse(typeof(IconMode), KeyVals["mode"], true);
                else
                    icon.Mode = IconMode.Normal;
                if (KeyVals.ContainsKey("hint"))
                {
                    icon.Text = KeyVals["hint"];
                }
                if (KeyVals.ContainsKey("cmd"))
                    icon.Cmd0 = KeyVals["cmd"];
                if (KeyVals.ContainsKey("cmd0"))
                    icon.Cmd0 = KeyVals["cmd0"];
                if (KeyVals.ContainsKey("cmd1"))
                    icon.Cmd1 = KeyVals["cmd1"];
                if (command == "dns-net-beryllium-status-ico_add")
                    connection.AddWidgit(icon);
                if (icon.Mode == IconMode.Hidden)
                    icon.Available = false;
                else
                    icon.Available = true;
                icon.DisplayStyle = ToolStripItemDisplayStyle.Image;
                icon.Alignment = ToolStripItemAlignment.Right;
                icon.Visible = true;
                
            }
            else if (System.Diagnostics.Debugger.IsAttached)
                throw new NotImplementedException(command); // It'll get printed to screen.
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            TrayImage.Images.AddStrip(Properties.Resources.Beryllium_Tray);
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported { get; set; }

        public enum IconMode {Normal, Hidden, Blink, On, Off};
        public class TrayIcon : ToolStripLabel
        {
            private BerylliumStatus statusPackage;
            public TrayIcon(BerylliumStatus package)
            {
                this.statusPackage = package;
            }

            public int Icon { get; set; }
            public int OnIcon { get; set; }   // for buttonmode
            public int OffIcon { get; set; }  // for buttonmode
            public IconMode Mode { get; set; }
            //public int Left { get; set; }
            //public int Width { get; set; }
            //public string Hint { get; set; }
            public string Cmd0 { get; set; }
            public string Cmd1 { get; set; }
            public ContextMenuStrip Menu { get; set; }
            public override System.Drawing.Image Image
            {
                get
                {
                    bool on = this.Mode == IconMode.Normal || this.Mode == IconMode.On;
                    if (this.Mode == IconMode.Blink)
                    {
                        on = true; //DateTime.Now.Second % 2 == 1;
                    }
                    if (on && OnIcon != 0)
                        return statusPackage.TrayImage.Images[OnIcon - 1];
                    else if (OffIcon != 0)
                        return statusPackage.TrayImage.Images[OffIcon - 1];
                    else
                        return statusPackage.TrayImage.Images[0];
                }
                set
                {
                    throw new NotSupportedException("Set the .Icon property instead.");
                }
            }
        }
    }
}

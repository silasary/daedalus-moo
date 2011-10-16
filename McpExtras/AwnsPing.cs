using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;
using System.Windows.Forms;
using Daedalus;

namespace McpExtras
{
    class AwnsPing : MCPPackage
    {
        Timer timer;
        private ToolStripLabel pinglabel;
        readonly MCPHandler Handler;
        bool supported;
        public bool Supported
        {
            get
            {
                return supported;
            }
            set
            {
                if (value)
                {
                    if (timer == null)
                    {
                        timer = new Timer() { Interval = 60 * 100, Enabled = true };
                        timer.Start();
                        timer.Tick += new EventHandler(timer_Tick);
                    }
                    if (pinglabel == null)
                    {
                        pinglabel = new ToolStripLabel("Awns-Ping") { Name = "Awns-ping", Alignment = ToolStripItemAlignment.Right };
                        ((MDIChild)Handler.CurrentConnection.Form).ToolStripItems.Add(pinglabel);
                    }
                }
                else
                {
                    timer.Dispose();
                    timer = null;
                }
                pinglabel.Visible = value;
                supported = value;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            string id = "singleton"; // We're lazy :/
            Handler.SendOOB("dns-com-awns-ping", MCPHandler.CreateKeyvals("id", id));
            Sent = DateTime.Now;
        }

        public AwnsPing(MCPHandler handler)
        {
            this.Handler = handler;
        }

        #region MCPPackage Members

        public void Disconnected()
        {
            Supported = false;
        }
        DateTime Sent;
        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-com-awns-ping") // Server pinging us.
                Handler.SendOOB("dns-com-awns-ping-reply", KeyVals); // Yes, sending it straight back works.
            else if (command == "dns-com-awns-ping-reply")
            {
                TimeSpan diff = DateTime.Now.Subtract(Sent);
                pinglabel.Text = diff.ToString();
            }
            else
                throw new NotImplementedException(command);
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            
        }

        public string PackageName
        {
            get { return "dns-com-awns-ping"; }
        }

        public string maxVer
        {
            get { return "1.0"; }
        }

        public string minVer
        {
            get { return "1.0"; }
        }

        #endregion
    }
}

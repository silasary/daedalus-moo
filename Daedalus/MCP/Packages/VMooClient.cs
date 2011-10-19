using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Daedalus.Core;
using System.Text.RegularExpressions;

namespace Daedalus.MCP.Packages
{
    class VMooClient : MCPPackage
    {
        MCPHandler Handler;
        public VMooClient(MCPHandler handler)
        {
            this.Handler = handler;
        }

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-vmoo-client"; }
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
            // #$#dns-com-vmoo-client-disconnect -1495338572 reason: "I don't like your face"
            if (command == "dns-com-vmoo-client-disconnect")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "reason"))
                    return;
                Handler.CurrentConnection.SetDisconnectReason(KeyVals["reason"]);
            }
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            Handler.CurrentConnection.Form.TextViewControl.Resize += new EventHandler(Form_ResizeEnd);
            SendScreenSize();
            string flags = "l"; // l = Links. m = Moops. p = proxy.
            string xvmooflags = ""; // x-vmoo-flags: m (multimedia), w (popup windows), a (unknown)
            Handler.SendOOB("dns-com-vmoo-client-info", MCPHandler.CreateKeyvals("name", MOO.Interop.Escape(Application.ProductName), "text-version", MOO.Interop.Escape(Application.ProductVersion), "internal-version", MOO.Interop.Escape(new Version(Application.ProductVersion).Build.ToString()), "flags", MOO.Interop.Escape(flags), "reg-id", MOO.Interop.Escape("0"),"x-vmoo-flags", MOO.Interop.Escape(xvmooflags)));
            Handler.CurrentConnection.ServicesDispatcher.RegisterMessageHandler(this.ParseLinks);
            Handler.CurrentConnection.ServicesDispatcher.RegisterLinkClickedHandler(LinkClicked);
        }

        void Form_ResizeEnd(object sender, EventArgs e)
        {
            SendScreenSize();
        }
        #endregion

        public void SendScreenSize()
        {
            Handler.SendOOB("dns-com-vmoo-client-screensize", MCPHandler.CreateKeyvals("Cols", Handler.CurrentConnection.Form.TextView.Columns.ToString(), "Rows", Handler.CurrentConnection.Form.TextView.Lines.ToString()));
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion
        public bool Supported { get; set; }
        #region @[Links]
        static Regex vmooLinkExp = new Regex(@"@\[(\w+):(.+)\](.+)@\[/\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // @[{1}:{2}]{3}@[/]
        public ColorMessage ParseLinks(ColorMessage colorMessage)
        {
            MatchCollection matches = vmooLinkExp.Matches(colorMessage.Text);
            if (matches.Count > 0)
            {
                Match match = matches[0];
                int index = match.Index;
                int length = match.Length;
                string link = match.Groups[0].Value;
                string protocol = match.Groups[1].Value;
                string command = match.Groups[2].Value;
                string text = match.Groups[3].Value;
                colorMessage.Remove(index, ("@[" + protocol + ":" + command + "]").Length);
                colorMessage.Remove(index + text.Length, "@[/]".Length);
                if (protocol == "ansi")
                {
                    //colorMessage.Colorize
                }
                else
                {
                    colorMessage.Linkify(index, text.Length, protocol + ":" + command);
                }
            }
            return colorMessage;
        }
        private bool LinkClicked(string link)
        {
            string proto = link.Split(':')[0].ToLower();
            link = link.Substring(proto.Length + 1);
            switch (proto)
            {
                case "exit":
                case "go": // I assume these are equal?
                    Handler.CurrentConnection.SendLine("GO " + link);
                    return true;
                case "look":
                    Handler.CurrentConnection.SendLine("LOOK " + link);
                    return true;
                case "join":
                    Handler.CurrentConnection.SendLine("@join " + link);
                    return true;
                case "moo":
                case "cmd":
                    Handler.CurrentConnection.SendLine(link);
                    return true;
            }
            return false;
        }
/*
    go exit look mailto http-link ansi

@[http://]<desc>@[/]

----------- @paste from CaPI --------------

Basis syntax:

@[[[       Letterlijk weergeven @[

@[[<code>:<waarde>]      (Wordt afgehandeld door de client)

@[[<moo-code>::<waarde>]         (Wordt via MCP aan de moo teruggestuurd)

@[[&<code>]       (Extended characters)

@[[&#<nr>]        (Extended characters)

@[[/]      Algemene afsluitcode

@[[off]      Algemene afsluitcode

@[[@]      Negeer verder op deze regel @[ codes

---------------------------------------



CaPI denkt, "<waarde> moet gequote worden als er een van deze tekens in zit: ]"\,;' ..." (ook de spatie)

*/
        #endregion
    }
}
/*
[+][Social] [------------Quadagh pastes to Social------------]
[+][Social] Form.ResizeEnd event leverages win32 WM_EXITSIZEMOVE message, so in
[+][Social] Net1.1, we can override Form's WndProc and create a public ResizeEnd
[+][Social] event, trigger this event in WM_EXITSIZEMOVE message, like below:
[+][Social]
[+][Social] public event EventHandler ResizeEnd;
[+][Social] private int WM_EXITSIZEMOVE=0x232;
[+][Social] protected override void WndProc(ref Message m)
[+][Social] {
[+][Social] if(m.Msg==WM_EXITSIZEMOVE)
[+][Social] {
[+][Social] if(ResizeEnd!=null)
[+][Social] {
[+][Social] this.ResizeEnd(this, new EventArgs());
[+][Social] }
[+][Social] }
[+][Social] base.WndProc (ref m);
[+][Social] }
[+][Social]
[+][Social] In usercontrol, we can just listen to this event:
[+][Social] /I write code to disable this code from running in designmode.
[+][Social] private void UserControl1_Load(object sender, System.EventArgs e)
[+][Social] {
[+][Social] if(!this.DesignMode)
[+][Social] {
[+][Social] ((Form1)this.Parent).ResizeEnd += new
[+][Social] EventHandler(UserControl1_ResizeEnd);
[+][Social] }
[+][Social] else
[+][Social] {
[+][Social] MessageBox.Show("design time");
[+][Social] }
[+][Social] }
[+][Social]
[+][Social] private void UserControl1_ResizeEnd(object sender, EventArgs e)
[+][Social] {
[+][Social] MessageBox.Show("Resize end");
[+][Social] }
[+][Social] [------------Quadagh pastes to Social------------]
*/
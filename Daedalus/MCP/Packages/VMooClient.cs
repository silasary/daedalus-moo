using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            throw new NotImplementedException();
        }

        public void Negotiated()
        {
            Handler.CurrentConnection.Form.TextViewControl.Resize += new EventHandler(Form_ResizeEnd);
            SendScreenSize();
            Handler.SendOOB("dns-com-vmoo-client-info", MCPHandler.CreateKeyvals("name", MOO.Interop.Escape(Application.ProductName), "text-version", MOO.Interop.Escape(Application.ProductVersion), "internal-version", MOO.Interop.Escape(new Version(Application.ProductVersion).Build.ToString()), "flags", MOO.Interop.Escape(""), "reg-id", MOO.Interop.Escape("0")));
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
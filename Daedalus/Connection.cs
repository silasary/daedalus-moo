using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chiroptera.Base;
using Chiroptera.Win;
using Daedalus.MCP;

namespace Daedalus
{
    public class Connection : IChiConsole, INetwork, Daedalus.IConnection
    {
        WorldForm form;
        BaseServicesDispatcher servicesDispatcher;
        CommandManager commandManager;
        Telnet telnet;
        SavedSession session;

        public SavedSession Session { get { return session; } }
        public BaseServicesDispatcher ServicesDispatcher { get { return servicesDispatcher; } }

        public WorldForm Form { get { return form; } }

        Queue<string> m_receiveQueue = new Queue<string>();
        bool m_receiveEventSent = false;

        delegate void ReceiveEventDelegate();

        MCPHandler m_MCP;

        string DisconnectReason;
        DateTime DisconnectReasonTime;

        public Connection(WorldForm form)
        {
            this.form = form;
            
            servicesDispatcher = new BaseServicesDispatcher(this);

            commandManager = new CommandManager(ServicesDispatcher);
            new DefaultCommands(commandManager, this);

            DisconnectReason = "Disconnected Too quickly.";
            DisconnectReasonTime = DateTime.Now;

            telnet = new Telnet();
            telnet.connectEvent += new Telnet.ConnectDelegate(_ConnectEvent);
            telnet.disconnectEvent += new Telnet.DisconnectDelegate(_DisconnectEvent);
            telnet.receiveEvent += new Telnet.ReceiveDelegate(_ReceiveEvent);
        //    telnet.promptEvent += new Telnet.PromptDelegate(_PromptEvent);
            telnet.telnetEvent += new Telnet.TelnetDelegate(_TelnetEvent);

            m_MCP = new MCPHandler(this);

            PluginModel.PluginLoader.DispatchNewConnection(this);
        }

        #region IChiConsole Members

        public void WriteLine(string str)
        {
            string[] lines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            AnsiTextStyle style = AnsiTextStyle.Empty;
            foreach (string line in lines)
            {
                ColorMessage msg = ColorMessage.CreateFromAnsi(line, style);
                WriteLine(msg);
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }

        public void WriteLine(ColorMessage colorMsg)
        {
            colorMsg = ServicesDispatcher.DispatchOutputMessage(colorMsg);

            if (colorMsg == null)
                return;

            Paragraph p = new Paragraph(colorMsg);

            form.ParagraphContainer.Add(p);
        }
        public void WriteSystemLine(string format, params object[] args)
        {
            WriteLine(new ColorMessage(">>> " + String.Format(format, args), new List<ColorMessage.MetaData>() { new ColorMessage.MetaData(0, Color.FromSystemColor(System.Drawing.Color.White), Color.FromSystemColor(System.Drawing.Color.Blue)) }));
        }
        public void WriteError(string format, params object[] args)
        {
            WriteLine(new ColorMessage("!>> " + String.Format(format, args), new List<ColorMessage.MetaData>() { new ColorMessage.MetaData(0, Color.FromSystemColor(System.Drawing.Color.White), Color.FromSystemColor(System.Drawing.Color.Blue)) }));
        }
        public void WriteLineLow(string format, params object[] args)
        {
            string str = String.Format(format, args);

            string[] lines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            AnsiTextStyle style = AnsiTextStyle.Empty;
            foreach (string line in lines)
            {
                ColorMessage msg = ColorMessage.CreateFromAnsi(line, style);
                form.ParagraphContainer.Add(new Paragraph(msg));
            }
        }

        public string Prompt
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string InputLine
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region INetwork Members

        public void Connect(string address, int port)
        {
            Disconnect();

            this.WriteSystemLine("Connecting to {0}:{1}", address, port);
            telnet.Connect(address, port);
            this.session = new SavedSession() { Server = address, Port = port.ToString() };
        }
        
        public void Connect(SavedSession session)
        {
            Disconnect();
            this.session = session;
            int port;
            this.WriteSystemLine("Connecting to {0}:{1}", session.Server, port = int.Parse(session.Port));
            telnet.Connect(session.Server, port);
        }

        public void Disconnect()
        {
            telnet.Disconnect();
        }

        public bool IsConnected
        {
            get { return telnet.IsConnected; }
        }

        public void SendLine(string str)
        {
            str = ServicesDispatcher.DispatchInputEvent(str);

            if (str == null)
                return;

            if (IsConnected)
                telnet.Send(str + "\n");
        }

        public void SendLine(string format, params object[] args)
        {
            SendLine(String.Format(format, args));
        }

        public void ReceiveLine(string str)
        {
            ReceiveEvent(str);
        }


        #endregion

        #region UI Management
        public void SetStatus(string status)
        {
            form.StatusLabel.Text = status;
            form.StatusLabel.Tag = new Chiroptera.Win.Paragraph.MetaData(0, System.Drawing.Color.Black, System.Drawing.Color.White);
            UpdateStatusbarData();
        }

        public void SetStatus(string status, object tag)
        {
            form.StatusLabel.Text = status;
            form.StatusLabel.Tag = tag;
            UpdateStatusbarData();
        }

        private void UpdateStatusbarData()
        {
            form.StatusLabel.ForeColor = ((Chiroptera.Win.Paragraph.MetaData)form.StatusLabel.Tag).m_fgColor;
        }

        public void AddWidgit(System.Windows.Forms.Control control)
        {
            if (form.SidebarCollapsed)
            {
                form.SidebarCollapsed = false;
                form.Sidebar.Width = 120;
            }
            form.Sidebar.Controls.Add(control);
            form.SidebarCollapsed = false;
        }
        public void AddWidgit(System.Windows.Forms.ToolStripItem toolstripitem)
        {
            form.AddWidgit(toolstripitem);
        }
        public void AddWidgit(System.Windows.Forms.ToolStripMenuItem menuItem)
        {
            if (form.menu == null)
                form.menu = new System.Windows.Forms.ToolStripMenuItem("World");
            form.menu.DropDownItems.Add(menuItem);
        }
        #endregion

        #region TelnetEvents
        void _ConnectEvent(Exception exception, string address, int port)
        {
            form.BeginInvoke(new Telnet.ConnectDelegate(ConnectEvent), new object[] { exception, address, port });
        }

        void ConnectEvent(Exception exception, string address, int port)
        {
            ServicesDispatcher.DispatchConnectEvent(exception);

            DisconnectReason = "Disconnected Too quickly.";
            DisconnectReasonTime = DateTime.Now;

            if (exception == null)
            {
                WriteSystemLine("Connected to {0}:{1}", address, port);
                SetStatus(String.Format("Connected to {0}:{1}", address, port));
                if (session.Username != "" && session.Password != "")
                    telnet.SendLine(session.LoginString.Replace("%u", session.Username).Replace("%p", session.Password));

            }
            else
            {
                WriteSystemLine("Connect failed to {0}:{1} : {2}", address, port, exception.Message);
                SetStatus(String.Format("Connect failed to {0}:{1} : {2}", address, port, exception.Message));
            }
        }

        // Transfers control to MainForm's thread
        void _DisconnectEvent(string address, int port)
        {
            Form.BeginInvoke(new Telnet.DisconnectDelegate(DisconnectEvent), new object[] { address, port });
        }            

        void DisconnectEvent(string address, int port)
        {
            ServicesDispatcher.DispatchDisconnectEvent();

            string reason = "";
            if (DateTime.Now.Subtract(DisconnectReasonTime).TotalSeconds < 5)
                reason = String.Format(" ({0})", DisconnectReason);

            WriteSystemLine("Disconnected from {0}:{1}{2}", address, port,reason);
            SetStatus( String.Format("Disconnected from {0}:{1}{2}", address, port, reason));
            if (reason != "" && Settings.Default.AutoReconnect)
                this.Connect(this.session);
        }

        // Transfers control to MainForm's thread
        void _ReceiveEvent(string data)
        {
            lock (m_receiveQueue)
            {
                m_receiveQueue.Enqueue(data);
                if (m_receiveEventSent == false)
                {
                    m_receiveEventSent = true;
                    Form.BeginInvoke(new ReceiveEventDelegate(ReceiveEventBulk), null);
                }
            }
        }

        AnsiTextStyle m_currentStyle = AnsiTextStyle.Empty;

        void ReceiveEventBulk()
        {
            string[] arr;

            lock (m_receiveQueue)
            {
                arr = m_receiveQueue.ToArray();
                m_receiveQueue.Clear();
                m_receiveEventSent = false;
            }

            //ChiConsole.WriteLineLow("Got {0} lines", arr.Length);
            foreach (string s in arr)
            {
                ReceiveEvent(s);
            }
        }

        void ReceiveEvent(string data)
        {
            string[] strs = data.Split('\n');

            foreach (string str in strs)
            {
#if DEBUG
                //if (m_showOutputDebug)
                //    ChiConsole.WriteLineLow("rcv: " + str.Replace("\x1b", "<esc>"));
#endif

                ColorMessage colorMsg = Ansi.ParseAnsi(str, ref m_currentStyle);

                colorMsg = ServicesDispatcher.DispatchReceiveColorMessage(colorMsg);

                if (colorMsg == null)
                    return;

                WriteLine(colorMsg);
            }
        }

        void _TelnetEvent(Telnet.TelnetCodes code, Telnet.TelnetOpts opt)
        {
            Form.BeginInvoke(new Telnet.TelnetDelegate(TelnetEvent), new object[] { code, opt });
        }

        void TelnetEvent(Telnet.TelnetCodes code, Telnet.TelnetOpts opt)
        {
            ServicesDispatcher.DispatchTelnetEvent(code, opt);

            if (code == Telnet.TelnetCodes.WILL && opt == Telnet.TelnetOpts.TELOPT_ECHO)
            {
                //m_mainWindow.PromptTextBox.PromptPassword = true;
            }
            else if (code == Telnet.TelnetCodes.WONT && opt == Telnet.TelnetOpts.TELOPT_ECHO)
            {
                //m_mainWindow.PromptTextBox.PromptPassword = false;
            }
        }
        #endregion

        internal void SetDisconnectReason(string p)
        {
            DisconnectReasonTime = DateTime.Now;
            DisconnectReason = p;
        }
    }
}

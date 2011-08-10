﻿using System;
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
        BaseServicesDispatcher ServicesDispatcher;
        Telnet telnet;
        SavedSession session;

        public SavedSession Session { get { return session; } }

        public WorldForm Form { get { return form; } }

        Queue<string> m_receiveQueue = new Queue<string>();
        Queue<string> m_receiveOOBQueue = new Queue<string>();
        bool m_receiveEventSent = false;
        bool m_receiveOOBEventSent = false;

        delegate void ReceiveEventDelegate();

        MCPHandler m_MCP;

        public Connection(WorldForm form)
        {
            this.form = form;
            
            ServicesDispatcher = new BaseServicesDispatcher();

            telnet = new Telnet();
            telnet.connectEvent += new Telnet.ConnectDelegate(_ConnectEvent);
            telnet.disconnectEvent += new Telnet.DisconnectDelegate(_DisconnectEvent);
            telnet.receiveEvent += new Telnet.ReceiveDelegate(_ReceiveEvent);
        //    telnet.promptEvent += new Telnet.PromptDelegate(_PromptEvent);
            telnet.telnetEvent += new Telnet.TelnetDelegate(_TelnetEvent);

            m_MCP = new MCPHandler(this);
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

            this.WriteLine("Connecting to {0}:{1}", address, port);
            telnet.Connect(address, port);
            this.session = new SavedSession() { Server = address, Port = port.ToString() };
        }
        
        public void Connect(SavedSession session)
        {
            Disconnect();
            this.session = session;
            int port;
            this.WriteLine("Connecting to {0}:{1}", session.Server, port = int.Parse(session.Port));
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
            form.ToolStripItems.Add(toolstripitem);
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

            if (exception == null)
            {
                WriteLine("Connected to {0}:{1}", address, port);
                SetStatus(String.Format("Connected to {0}:{1}", address, port));
                if (session.Username != "" && session.Password != "")
                    telnet.SendLine(session.LoginString.Replace("%u", session.Username).Replace("%p", session.Password));

            }
            else
            {
                WriteLine("Connect failed to {0}:{1} : {2}", address, port, exception.Message);
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

            WriteLine("Disconnected from {0}:{1}", address, port);
            SetStatus( String.Format("Disconnected from {0}:{1}", address, port));
            //m_mainWindow.PromptTextBox.Prompt = "";
            //m_mainWindow.PromptTextBox.PromptPassword = false;
        }

        // Transfers control to MainForm's thread
        void _ReceiveEvent(string data)
        {
            lock (m_receiveQueue)
            {
                if (data.StartsWith("#$#")) // Out of Band messages.
                {
                    lock (m_receiveOOBQueue)
                    {
                        m_receiveOOBQueue.Enqueue(data);
                        if (m_receiveOOBEventSent == false)
                        {
                            m_receiveOOBEventSent = true;
                            Form.BeginInvoke(new ReceiveEventDelegate(ReceiveOOBBulk), null);
                        }
                    }
                }
                else
                {
                    if (data.StartsWith("#$\"")) // unquote quoted OOB lines.
                    {
                        data = data.Remove(0, 3);
                    }
                    m_receiveQueue.Enqueue(data);
                    if (m_receiveEventSent == false)
                    {
                        m_receiveEventSent = true;
                        Form.BeginInvoke(new ReceiveEventDelegate(ReceiveEventBulk), null);
                    }
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

        void ReceiveOOBBulk()
        {
            string[] arr = new string[] { };

            lock (m_receiveOOBQueue)
            {
                try
                {
                    arr = m_receiveOOBQueue.ToArray();
                    m_receiveOOBQueue.Clear();
                }
                catch { }
                m_receiveOOBEventSent = false;
            }

            //ChiConsole.WriteLineLow("Got {0} OOB lines", arr.Length);
            foreach (string s in arr)
            {
                try
                {
                    m_MCP.ReceiveOOB(s);
                }
                catch { } // We don't care if an OOB line fails.
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
    }
}
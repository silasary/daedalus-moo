﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chiroptera.Base;

namespace Daedalus.MCP.Packages
{
    class Multiplex : MCPPackage
    {
        MCPHandler Handler;

        public Multiplex(MCPHandler handler)
        {
            this.Handler = handler;
        }

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-uk-co-thc-gaming-multiplex"; }
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
            switch (command)
            {
                case ("dns-uk-co-thc-gaming-multiplex-open"):
                    if (!MCPHandler.ContainsKeys(KeyVals, "name", "tag"))
                        return;
                    WorldForm wf = new WorldForm();
                    wf._connection = new MultiplexConnection(wf, this.WindowCommand, KeyVals["tag"]);
                    windows.Add(KeyVals["tag"], (MultiplexConnection)wf._connection);
                    break;
                default:
                    break;
            }
        }

        public void Negotiated()
        {
            windows = new Dictionary<string, MultiplexConnection>();
        }

        #endregion
        Dictionary<string, MultiplexConnection> windows;

        public void WindowCommand(string tag, string line)
        {
            Handler.SendOOB("dns-uk-co-thc-gaming-multiplex-command", MCPHandler.CreateKeyvals("_data-tag", tag, "line", line));
        }
    }

    public class MultiplexConnection : IConnection
    {
        Connection console;
        public delegate void SendCommand(string tag,string line);
        SendCommand cmd;
        readonly string tag;
        public MultiplexConnection(WorldForm form, SendCommand cmd, string tag)
        {
            console = new Connection(form);
            this.cmd = cmd;
            this.tag = tag;
        }
        #region IChiConsole Members

        public void WriteLine(string str)
        {
            console.WriteLine(str);
        }

        public void WriteLine(string format, params object[] args)
        {
            console.WriteLine(format, args);
        }

        public void WriteLine(ColorMessage msg)
        {
            console.WriteLine(msg);
        }

        public void WriteLineLow(string format, params object[] args)
        {
            console.WriteLineLow(format, args);
        }

        public string Prompt
        {
            get
            {
                return console.Prompt;
            }
            set
            {
                console.Prompt = value;
            }
        }

        public string InputLine
        {
            get
            {
                return console.InputLine;
            }
            set
            {
                console.InputLine = value;
            }
        }

        #endregion

        #region INetwork Members

        public void Connect(string address, int port)
        {
            console.Form._connection.Connect(address, port);
        }
        public void Connect(SavedSession session)
        {
            console.Form._connection.Connect(session);
        }
        public void Disconnect()
        {
            console.Form._connection.Disconnect();
        }

        public bool IsConnected
        {
            get { return console.Form._connection.IsConnected; }
        }

        public void SendLine(string str)
        {
            cmd.Invoke(tag,str);
        }

        public void SendLine(string format, params object[] args)
        {
            cmd.Invoke(tag, String.Format(format, args));
        }

        public void ReceiveLine(string str)
        {
            this.WriteLine(str);
        }

        #endregion

        #region IConnection Members

        public void AddWidgit(System.Windows.Forms.Control control)
        {
            console.AddWidgit(control);
        }

        public void AddWidgit(System.Windows.Forms.ToolStripItem toolstripitem)
        {
            console.AddWidgit(toolstripitem);
        }

        public WorldForm Form
        {
            get { return console.Form; }
        }

        public SavedSession Session
        {
            get { return null; }
        }

        public void SetStatus(string status)
        {
            console.SetStatus(status);
        }

        #endregion
    }
}

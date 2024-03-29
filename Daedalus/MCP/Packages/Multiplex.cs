﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.Core;
using Daedalus.MOO;

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
                    wf.Text = KeyVals["name"];
                    Handler.CurrentConnection.Form.Invoke(new Action(() => wf.Show())); // We need it on the UI thread.
                    break;
                case ("dns-uk-co-thc-gaming-multiplex-msg"):
                    if (MCPHandler.ContainsKeys(KeyVals, "tag"))
                        if (!windows.ContainsKey(KeyVals["tag"]))
                        {
                            Handler.SendOOB("dns-uk-co-thc-gaming-multiplex-close", MCPHandler.CreateKeyvals("tag", KeyVals["tag"]));
                            return;
                        }
                    if (MCPHandler.ContainsKeys(KeyVals, "tag", "_data-tag", "line*")) // They're sending more than one.
                        Handler.RegisterMultilineHandler(KeyVals["_data-tag"], "dns-uk-co-thc-gaming-multiplex-mlmsg-" + KeyVals["tag"]);
                    if (!MCPHandler.ContainsKeys(KeyVals, "line", "tag"))
                        return;
                    windows[KeyVals["tag"]].WriteLine(KeyVals["line"]);
                    break;
                case ("dns-uk-co-thc-gaming-multiplex-cls"):
                    if (!MCPHandler.ContainsKeys(KeyVals, "tag"))
                        return;
                    for (int i = 0; i < windows[KeyVals["tag"]].Form.TextView.Lines; i++)
                    {
                        windows[KeyVals["tag"]].WriteLine("\n");
                    }
                    break;
                case ("dns-uk-co-thc-gaming-multiplex-close"):
                    windows[KeyVals["tag"]].Form.Close();
                    windows.Remove(KeyVals["tag"]);
                    break;
                default:
                    if (command.StartsWith("dns-uk-co-thc-gaming-multiplex-mlmsg-"))
                    {
                        string tag = command.Substring("dns-uk-co-thc-gaming-multiplex-mlmsg-".Length);
                        windows[tag].WriteLine(KeyVals["line"]);
                    }
                    break;
            }
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            windows = new Dictionary<string, MultiplexConnection>();
        }

        #endregion
        Dictionary<string, MultiplexConnection> windows;

        public void WindowCommand(string tag, string line)
        {
            Handler.SendOOB("dns-uk-co-thc-gaming-multiplex-command", MCPHandler.CreateKeyvals("tag", tag, "line", Interop.Escape(line)));
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            foreach (MultiplexConnection win in windows.Values)
            {
                win.Form.Close();
            }
            windows.Clear();
        }

        #endregion
        public bool Supported { get; set; }
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

        public void WriteSystemLine(string format, params object[] args)
        {
            console.WriteSystemLine(format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            console.WriteError(format, args);
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
            
        }
        public void Connect(SavedSession session)
        {
                    }
        public void Disconnect()
        {
            
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
        public void SetStatus(string status, object metadata)
        {
            console.SetStatus(status, metadata);
        }
        public BaseServicesDispatcher ServicesDispatcher
        {
            get { return console.ServicesDispatcher; }
        }

        #endregion
    }
}

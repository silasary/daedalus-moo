﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Daedalus.MCP.Packages
{
    class AwnsStatus :MCPPackage
    {
        Connection connection;
        public AwnsStatus(MCPHandler handler)
        {
            this.connection = handler.CurrentConnection;
        }
        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-awns-status"; }
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
            if (!MCPHandler.ContainsKeys(KeyVals, "text"))
                return;
            connection.SetStatus(KeyVals["text"]);
        }

        public void Negotiated()
        {
        }

        #endregion

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion


        public bool Supported { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Chiroptera.Win.MCP.Packages
{
    class AwnsStatus :MCPPackage
    {

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
            ClientCore.s_clientCore.MainWindow.StatusText.Text = KeyVals["text"];
        }

        public void Negotiated()
        {
        }

        #endregion
    }
}

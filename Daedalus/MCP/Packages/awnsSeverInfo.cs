using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.MCP.Packages
{
    class awnsSeverInfo : MCPPackage
    {
        MCPHandler handler;
        public awnsSeverInfo(MCPHandler handler)
        {
            this.handler = handler;
        }

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-awns-serverinfo"; }
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
            if (MCPHandler.ContainsKeys(KeyVals, "home_url") && !String.IsNullOrEmpty(KeyVals["home_url"]))
                handler.CurrentConnection.Session.HomePage = KeyVals["home_url"];
            // Same with Help url?
            
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            handler.SendOOB("dns-com-awns-serverinfo-get", new Dictionary<string, string>());
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported {get;set;}

        #endregion
    }
}

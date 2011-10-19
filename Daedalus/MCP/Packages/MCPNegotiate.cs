using System;
using System.Collections.Generic;
using System.Text;

namespace Daedalus.MCP.Packages
{
    class MCPNegotiate : MCPPackage
    {
        MCPHandler Handler;
        public MCPNegotiate(MCPHandler handler)
        {
            this.Handler = handler;
        }


        #region MCPPackage Members

        public string PackageName
        {
            get { return "mcp-negotiate"; }
        }

        public string minVer
        {
            get { return "1.0"; }
        }

        public string maxVer
        {
            get { return "2.0"; }
        }

        public void Negotiated(string MinVersion, string MaxVersion) { } // We don't need to do anything.
        #endregion

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "mcp-negotiate-can")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "package", "min-version", "max-version"))
                    return;
                MCPPackage package = Handler.FindPackage(KeyVals["package"], KeyVals["min-version"], KeyVals["max-version"]);
                if (package == null)
                {
                    Daedalus.Core.ChiConsole.WriteError("MCP Package not found: " + KeyVals["package"] + "[" + KeyVals["min-version"] + "," + KeyVals["max-version"] + "]");
                    return;
                }
                package.Supported = true;
                try
                {
                    package.Negotiated(minVer,maxVer);
                }
                catch (NotImplementedException) { }
            }
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion
        public bool Supported { get { return true; } set { } }
    }
}

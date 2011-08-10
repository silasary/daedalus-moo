using System;
using System.Collections.Generic;
using System.Text;

namespace Chiroptera.Win.MCP.Packages
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

        public void Negotiated() { } // We don't need to do anything.
        #endregion

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "mcp-negotiate-can")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "package", "min-version", "max-version"))
                    return;
                MCPPackage package = Handler.FindPackage(KeyVals["package"]);
                if (package == null)
                {
                    Chiroptera.Base.ChiConsole.WriteError("MCP Package not found: " + KeyVals["package"]);
                    return;
                }
                try
                {
                    package.Negotiated();
                }
                catch { }
            }
        }
    }
}

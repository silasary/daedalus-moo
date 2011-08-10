using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chiroptera.Win.MCP.Packages
{
    class VMooSmartComplete : MCPPackage
    {
        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-vmoo-smartcomplete"; }
        }

        public string minVer
        {
            get { return "0.0"; }
        }

        public string maxVer
        {
            get { return "0.0"; }
        }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            
        }

        public void Negotiated()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

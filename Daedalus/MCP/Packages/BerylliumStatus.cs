using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.MCP.Packages
{
    class BerylliumStatus : MCPPackage
    {
        Connection connection;
        public BerylliumStatus(MCPHandler handler)
        {
            this.connection = handler.CurrentConnection;
        }
        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-net-beryllium-status"; }
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
            if (command == "dns-net-beryllium-status-msg_force")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "text", "cmd"))
                    return;
                connection.SetStatus(KeyVals["text"], new Chiroptera.Win.Paragraph.MetaData(0, System.Drawing.Color.Black, System.Drawing.Color.White) { linkurl = KeyVals["cmd"] });
            }
        }

        public void Negotiated()
        {
            throw new NotImplementedException();
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion

       
    }
}
